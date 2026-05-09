using AshryverBot.Database.Entities;
using AshryverBot.Database.Repositories;

namespace AshryverBot.Infrastructure.Twitch.Tokens;

public static class TwitchTokenMapping
{
    public static TwitchTokenInfo ToInfo(this TwitchTokenEntity entity) => new(
        entity.TwitchUserId,
        entity.Login,
        entity.DisplayName,
        entity.Email,
        entity.AccessToken,
        entity.RefreshToken,
        entity.ExpiresAt,
        string.IsNullOrWhiteSpace(entity.Scopes)
            ? []
            : entity.Scopes.Split(' ', StringSplitOptions.RemoveEmptyEntries),
        entity.IsBotAccount);

    extension(TwitchTokenInfo info)
    {
        public TwitchTokenEntity ToNewEntity(DateTimeOffset now) => new()
        {
            TwitchUserId = info.TwitchUserId,
            Login = info.Login,
            DisplayName = info.DisplayName,
            Email = info.Email,
            AccessToken = info.AccessToken,
            RefreshToken = info.RefreshToken,
            ExpiresAt = info.ExpiresAt,
            Scopes = string.Join(' ', info.Scopes),
            IsBotAccount = info.IsBotAccount,
            CreatedAt = now,
            UpdatedAt = now,
        };

        public void ApplyTo(TwitchTokenEntity entity, DateTimeOffset now)
        {
            entity.Login = info.Login;
            entity.DisplayName = info.DisplayName;
            entity.Email = info.Email;
            entity.AccessToken = info.AccessToken;
            entity.RefreshToken = info.RefreshToken;
            entity.ExpiresAt = info.ExpiresAt;
            entity.Scopes = string.Join(' ', info.Scopes);
            entity.IsBotAccount = info.IsBotAccount;
            entity.UpdatedAt = now;
        }
    }

    public static async Task UpsertAsync(
        this ITwitchTokenRepository repository,
        TwitchTokenInfo info,
        DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        var existing = await repository.GetAsync(info.TwitchUserId, cancellationToken);
        if (existing is null)
        {
            await repository.CreateAsync(info.ToNewEntity(now), cancellationToken);
        }
        else
        {
            info.ApplyTo(existing, now);
            await repository.UpdateAsync(existing, cancellationToken);
        }
    }
}
