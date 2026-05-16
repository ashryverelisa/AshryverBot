using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories;
using AshryverBot.Web.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AshryverBot.Web.Components.Pages;

public partial class Commands
{
    [Inject] public ICommandRepository CommandRepository { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    private string _search = string.Empty;
    private bool _loading = true;
    private List<CommandEntity> _commands = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var commands = await CommandRepository.ListAsync();
            _commands = commands.ToList();
        }
        finally
        {
            _loading = false;
        }
    }

    private IEnumerable<CommandEntity> FilteredCommands =>
        string.IsNullOrWhiteSpace(_search)
            ? _commands
            : _commands.Where(c =>
                c.Name.Contains(_search, StringComparison.OrdinalIgnoreCase) ||
                c.Response.Contains(_search, StringComparison.OrdinalIgnoreCase));

    private static Color PermissionColor(CommandRole role) => role switch
    {
        CommandRole.Broadcaster => Color.Error,
        CommandRole.Moderator   => Color.Warning,
        CommandRole.Vip         => Color.Tertiary,
        CommandRole.Subscriber  => Color.Secondary,
        _                       => Color.Default,
    };

    private async Task OnToggleEnabled(CommandEntity command, bool value)
    {
        var previous = command.IsEnabled;
        command.IsEnabled = value;
        command.UpdatedAt = DateTimeOffset.UtcNow;
        try
        {
            await CommandRepository.UpdateAsync(command);
        }
        catch (Exception ex)
        {
            command.IsEnabled = previous;
            Snackbar.Add($"Failed to update command: {ex.Message}", Severity.Error);
        }
    }

    private async Task OpenAddDialog()
    {
        var draft = new CommandEntity
        {
            Name = string.Empty,
            Response = string.Empty,
            CooldownSeconds = 10,
            RequiredRole = CommandRole.Everyone,
            IsEnabled = true,
        };

        var parameters = new DialogParameters
        {
            [nameof(CommandEditDialog.Command)] = draft,
            [nameof(CommandEditDialog.IsNew)] = true,
        };

        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CommandEditDialog>("New command", parameters, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled || result.Data is not CommandEntity entity)
            return;

        var now = DateTimeOffset.UtcNow;
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = now;
        entity.UpdatedAt = now;
        entity.UsageCount = 0;

        try
        {
            await CommandRepository.CreateAsync(entity);
            _commands.Add(entity);
            _commands = _commands.OrderBy(c => c.Name).ToList();
            Snackbar.Add($"Command !{entity.Name} created.", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to create command: {ex.Message}", Severity.Error);
        }
    }

    private async Task OpenEditDialog(CommandEntity command)
    {
        var parameters = new DialogParameters
        {
            [nameof(CommandEditDialog.Command)] = command,
            [nameof(CommandEditDialog.IsNew)] = false,
        };

        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CommandEditDialog>($"Edit !{command.Name}", parameters, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled || result.Data is not CommandEntity edited)
            return;

        edited.UpdatedAt = DateTimeOffset.UtcNow;

        try
        {
            await CommandRepository.UpdateAsync(edited);

            var index = _commands.FindIndex(c => c.Id == edited.Id);
            if (index >= 0) _commands[index] = edited;
            _commands = _commands.OrderBy(c => c.Name).ToList();

            Snackbar.Add($"Command !{edited.Name} updated.", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to update command: {ex.Message}", Severity.Error);
        }
    }

    private async Task Delete(CommandEntity command)
    {
        var confirmed = await DialogService.ShowMessageBoxAsync(
            "Delete command",
            $"Are you sure you want to delete !{command.Name}?",
            yesText: "Delete",
            cancelText: "Cancel");

        if (confirmed != true) return;

        try
        {
            await CommandRepository.DeleteAsync(command.Id);
            _commands.Remove(command);
            Snackbar.Add($"Command !{command.Name} deleted.", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to delete command: {ex.Message}", Severity.Error);
        }
    }
}