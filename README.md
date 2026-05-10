# AshryverBot

A self-hosted Twitch bot with a Blazor Server admin web UI.

> **Status:** early stage. The web app, Twitch OAuth login, token persistence, and database layer are in place. The Helix client and the actual chat bot runtime are still scaffolding.

## Tech stack

- **.NET 10** / C#
- **Blazor Server** with **MudBlazor 9** for the web UI
- **PostgreSQL 16** via **EF Core 10** + **Npgsql 10**
- **Twitch OAuth** via `AspNet.Security.OAuth.Twitch`
- **Docker Compose** for the runtime

## Solution layout

The solution file `AshrverBot.sln` lives in [`src/`](src) and contains four projects:

| Project | Purpose |
| --- | --- |
| `AshryverBot.Web` | Blazor Server app, MudBlazor UI, Twitch OAuth login (`/login`, `POST /logout`), startup composition root. |
| `AshryverBot.Twitch` | Twitch HTTP clients. `TwitchOAuthClient` talks to `id.twitch.tv/oauth2` (refresh & revoke). `TwitchClient` (Helix) is a stub. |
| `AshryverBot.Database` | EF Core `ApplicationDbContext`, entities, repositories, migrations. Registered via `AddAshryverBotDatabase(IConfiguration)`. |
| `AshryverBot.Infrastructure` | `TwitchTokenRefresher` (5-minute safety window) and entity↔DTO mapping. |

### Domain model

- **`TwitchTokenEntity`** - persisted OAuth tokens (access/refresh, expiry, scopes, bot-account flag).
- **`CommandEntity`** - chat command (name, response, cooldown, `RequiredRole`, usage counter, timestamps).
- **`CommandRole`** - `Everyone | Subscriber | Vip | Moderator | Broadcaster`.

## Configuration

Configuration keys live in `AshryverBot.Web/appsettings.json`. For local development, create `appsettings.local.json` next to it (loaded as an optional override and gitignored).

```json
{
  "ConnectionStrings": {
    "AshryverBot": "Host=localhost;Port=5432;Database=ashryverbot;Username=ashryverbot;Password=ashryverbot"
  },
  "DataProtection": {
    "KeyPath": ""
  },
  "Twitch": {
    "ClientId": "<your-client-id>",
    "ClientSecret": "<your-client-secret>",
    "BotUserId": "<twitch-user-id-of-the-bot-account>"
  }
}
```

- `ConnectionStrings:AshryverBot` - Npgsql connection string.
- `DataProtection:KeyPath` - when set, ASP.NET Data Protection keys are persisted to that directory (required for stable cookie/token encryption across restarts).
- `Twitch:ClientId` / `Twitch:ClientSecret` — credentials of your Twitch application.
- `Twitch:BotUserId` - the Twitch user id of the account the bot will run as.

Register a Twitch application at <https://dev.twitch.tv/console/apps>. The OAuth callback Blazor uses by default is `https://<host>/signin-twitch`.

## Running locally

### With Docker (recommended)

The compose file in [`src/`](src) runs the web app and a PostgreSQL 16 container.

```bash
cd src
docker compose up --build
```

- Web UI: <http://localhost:8080>
- Postgres password is taken from `POSTGRES_PASSWORD` (defaults to `ashryverbot`).
- Data Protection keys are persisted in the `dataprotection-keys` volume; Postgres data in `postgres-data`.
- Twitch credentials are **not** baked into the compose file — provide them via `appsettings.local.json` (which the prod Dockerfile excludes from the image), environment variables, or your own override.

### Without Docker

1. Start a PostgreSQL instance and put the connection string in `appsettings.local.json`.
2. Fill in the `Twitch:*` keys.
3. Run the web app:

```bash
cd src
dotnet run --project AshryverBot.Web
```

Migrations are applied automatically on startup (`db.Database.MigrateAsync()` in `Program.cs`).

## Database migrations

Migrations live in `AshryverBot.Database/Migrations`. To add a new one:

```bash
cd src
dotnet ef migrations add <Name> \
    --project AshryverBot.Database \
    --startup-project AshryverBot.Web
```

## Authentication flow

- `GET /login` issues a Twitch OAuth challenge requesting the scopes listed in `TwitchScopes.All`.
- On callback, `OnCreatingTicket` persists the access/refresh tokens to the database via `ITwitchTokenRepository`.
- On every request, `OnValidatePrincipal` refreshes the token if it is close to expiry; if the refresh fails the principal is rejected.
- `POST /logout` clears the cookie.

## Roadmap

- Implement Twitch Helix endpoints in `TwitchClient`.
- Background service that connects the bot account to chat and reacts to commands.
- Encrypt access/refresh tokens at rest (e.g. via `IDataProtector`).
- Domain and test projects.