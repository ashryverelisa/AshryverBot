using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories.Interfaces;
using AshryverBot.Infrastructure.Chat;
using AshryverBot.Infrastructure.Chat.Commands;
using AshryverBot.Infrastructure.Chat.Interfaces;
using AshryverBot.Infrastructure.CommandStats.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Xunit;

namespace AshryverBot.Infrastructure.Tests.Chat;

public class ChatCommandDispatcherTests
{
    private readonly ICommandRepository _repository = Substitute.For<ICommandRepository>();
    private readonly IChatResponder _responder = Substitute.For<IChatResponder>();
    private readonly ICommandStatsWriter _commandStats = Substitute.For<ICommandStatsWriter>();
    private readonly FakeTimeProvider _time = new(new DateTimeOffset(2026, 5, 17, 12, 0, 0, TimeSpan.Zero));

    private ChatCommandDispatcher BuildDispatcher(params IChatCommand[] staticCommands)
    {
        var services = new ServiceCollection();
        services.AddSingleton(_repository);
        services.AddSingleton(_responder);
        foreach (var cmd in staticCommands)
        {
            services.AddSingleton(cmd);
        }
        var scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
        return new ChatCommandDispatcher(scopeFactory, _time, _commandStats, NullLogger<ChatCommandDispatcher>.Instance);
    }

    private static ChatMessage MessageWithText(string text) =>
        new("broadcaster", "chatter", "chatter_login", "Chatter", "msg-1", text);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("hello world")]
    [InlineData("!")]
    [InlineData("!   ")]
    public async Task Non_command_text_is_ignored(string text)
    {
        var dispatcher = BuildDispatcher();

        await dispatcher.DispatchAsync(MessageWithText(text), CancellationToken.None);

        await _repository.DidNotReceiveWithAnyArgs().GetByNameAsync(default!, default);
        await _responder.DidNotReceiveWithAnyArgs().SendAsync(default!, default!, default);
    }

    [Fact]
    public async Task Static_command_is_executed_and_db_is_not_consulted()
    {
        var staticCmd = Substitute.For<IChatCommand>();
        staticCmd.Name.Returns("hello");
        var dispatcher = BuildDispatcher(staticCmd);

        await dispatcher.DispatchAsync(MessageWithText("!hello"), CancellationToken.None);

        await staticCmd.Received(1).ExecuteAsync(
            Arg.Is<ChatCommandContext>(c => c.Arguments.Count == 0),
            Arg.Any<CancellationToken>());
        await _repository.DidNotReceiveWithAnyArgs().GetByNameAsync(default!, default);
    }

    [Fact]
    public async Task Static_command_name_match_is_case_insensitive()
    {
        var staticCmd = Substitute.For<IChatCommand>();
        staticCmd.Name.Returns("WatchTime");
        var dispatcher = BuildDispatcher(staticCmd);

        await dispatcher.DispatchAsync(MessageWithText("!watchtime"), CancellationToken.None);

        await staticCmd.Received(1).ExecuteAsync(Arg.Any<ChatCommandContext>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Arguments_are_parsed_split_and_trimmed()
    {
        var staticCmd = Substitute.For<IChatCommand>();
        staticCmd.Name.Returns("say");
        var dispatcher = BuildDispatcher(staticCmd);

        await dispatcher.DispatchAsync(MessageWithText("!say  hello   world  "), CancellationToken.None);

        await staticCmd.Received(1).ExecuteAsync(
            Arg.Is<ChatCommandContext>(c =>
                c.Arguments.Count == 2 &&
                c.Arguments[0] == "hello" &&
                c.Arguments[1] == "world"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Static_command_exception_is_swallowed()
    {
        var staticCmd = Substitute.For<IChatCommand>();
        staticCmd.Name.Returns("boom");
        staticCmd.ExecuteAsync(Arg.Any<ChatCommandContext>(), Arg.Any<CancellationToken>())
            .Returns<Task>(_ => throw new InvalidOperationException("boom"));
        var dispatcher = BuildDispatcher(staticCmd);

        var ex = await Record.ExceptionAsync(() =>
            dispatcher.DispatchAsync(MessageWithText("!boom"), CancellationToken.None));

        Assert.Null(ex);
    }

    [Fact]
    public async Task Db_command_disabled_is_not_sent()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello", IsEnabled = false });

        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);

        await _responder.DidNotReceiveWithAnyArgs().SendAsync(default!, default!, default);
    }

    [Fact]
    public async Task Db_command_unknown_returns_silently()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((CommandEntity?)null);

        await dispatcher.DispatchAsync(MessageWithText("!nope"), CancellationToken.None);

        await _responder.DidNotReceiveWithAnyArgs().SendAsync(default!, default!, default);
    }

    [Fact]
    public async Task Db_command_enabled_sends_response()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello there", IsEnabled = true });

        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);

        await _responder.Received(1).SendAsync("broadcaster", "hello there", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Db_command_on_cooldown_is_skipped_within_window()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello", IsEnabled = true, CooldownSeconds = 10 });

        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);
        _time.Advance(TimeSpan.FromSeconds(5));
        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);

        await _responder.Received(1).SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Db_command_cooldown_expired_allows_second_send()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello", IsEnabled = true, CooldownSeconds = 10 });

        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);
        _time.Advance(TimeSpan.FromSeconds(11));
        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);

        await _responder.Received(2).SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Db_command_without_cooldown_never_throttles()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello", IsEnabled = true, CooldownSeconds = 0 });

        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);
        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);
        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);

        await _responder.Received(3).SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Db_command_responder_exception_is_swallowed()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello", IsEnabled = true });
        _responder.SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns<Task>(_ => throw new InvalidOperationException("send failed"));

        var ex = await Record.ExceptionAsync(() =>
            dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None));

        Assert.Null(ex);
    }

    [Fact]
    public async Task Static_command_execution_is_recorded()
    {
        var staticCmd = Substitute.For<IChatCommand>();
        staticCmd.Name.Returns("hello");
        var dispatcher = BuildDispatcher(staticCmd);

        await dispatcher.DispatchAsync(MessageWithText("!hello"), CancellationToken.None);

        _commandStats.Received(1).RecordExecution();
    }

    [Fact]
    public async Task Static_command_execution_is_recorded_even_when_command_throws()
    {
        var staticCmd = Substitute.For<IChatCommand>();
        staticCmd.Name.Returns("boom");
        staticCmd.ExecuteAsync(Arg.Any<ChatCommandContext>(), Arg.Any<CancellationToken>())
            .Returns<Task>(_ => throw new InvalidOperationException("boom"));
        var dispatcher = BuildDispatcher(staticCmd);

        await dispatcher.DispatchAsync(MessageWithText("!boom"), CancellationToken.None);

        _commandStats.Received(1).RecordExecution();
    }

    [Fact]
    public async Task Db_command_execution_is_recorded()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello", IsEnabled = true });

        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);

        _commandStats.Received(1).RecordExecution();
    }

    [Fact]
    public async Task Db_command_disabled_is_not_recorded()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello", IsEnabled = false });

        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);

        _commandStats.DidNotReceive().RecordExecution();
    }

    [Fact]
    public async Task Db_command_unknown_is_not_recorded()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((CommandEntity?)null);

        await dispatcher.DispatchAsync(MessageWithText("!nope"), CancellationToken.None);

        _commandStats.DidNotReceive().RecordExecution();
    }

    [Fact]
    public async Task Db_command_skipped_by_cooldown_is_not_recorded()
    {
        var dispatcher = BuildDispatcher();
        _repository.GetByNameAsync("hi", Arg.Any<CancellationToken>())
            .Returns(new CommandEntity { Name = "hi", Response = "hello", IsEnabled = true, CooldownSeconds = 10 });

        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);
        _time.Advance(TimeSpan.FromSeconds(5));
        await dispatcher.DispatchAsync(MessageWithText("!hi"), CancellationToken.None);

        _commandStats.Received(1).RecordExecution();
    }

    [Fact]
    public async Task Non_command_text_is_not_recorded()
    {
        var dispatcher = BuildDispatcher();

        await dispatcher.DispatchAsync(MessageWithText("hello there"), CancellationToken.None);

        _commandStats.DidNotReceive().RecordExecution();
    }
}
