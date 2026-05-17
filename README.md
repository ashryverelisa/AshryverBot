# AshryverBot

A self-hosted Twitch bot with a Blazor Server admin web UI.

> **Status:** functional. OAuth login, EventSub-WebSocket chat ingest, a `!watchtime` chat command, live bot- and stream-status, watchtime tracking and a command CRUD UI are in place.

## Tech stack

- **.NET 10** / C#
- **Blazor Server** with **MudBlazor 9** for the web UI
- **PostgreSQL 16** via **EF Core 10** + **Npgsql 10**
- **Twitch OAuth** via `AspNet.Security.OAuth.Twitch`
- **Twitch Helix** + **EventSub WebSocket** (custom client)
- **Docker Compose** for the runtime
- **xUnit** for unit tests

## Solution layout

The solution file `AshrverBot.sln` lives in [`src/`](src) and contains four projects plus three test projects:

| Project | Purpose |
| --- | --- |
| `AshryverBot.Web` | Blazor Server app, MudBlazor UI, Twitch OAuth login (`/login`, `POST /logout`), startup composition root, admin pages (Home/Chat/Commands). |
| `AshryverBot.Twitch` | Helix HTTP wrapper (`ITwitchClient`), OAuth client, full set of Helix API services (Chat, Streams, EventSub, …), rate-limit handler, and a generic EventSub-WebSocket framework (`EventSubWebSocketClient`, `IEventSubHandler`). |
| `AshryverBot.Database` | EF Core `ApplicationDbContext`, entities, repositories, migrations. Registered via `AddAshryverBotDatabase(IConfiguration)`. |
| `AshryverBot.Infrastructure` | Token refresher, EventSub hosted service, watchtime poller, chat dispatcher/responder, `!watchtime` command, command CRUD service, bot-status & stream-stats trackers. |
| `*.Tests` | Unit tests for the Database, Infrastructure and Twitch projects. |

### Domain model

- **`TwitchTokenEntity`** - persisted OAuth tokens (access/refresh, expiry, scopes, bot-account flag).
- **`CommandEntity`** - chat command (name, response, cooldown, `RequiredRole`, usage counter, timestamps).
- **`CommandRole`** - `Everyone | Subscriber | Vip | Moderator | Broadcaster`.
- **`WatchtimeEntity`** - per-user watchtime aggregate (`TwitchUserId` unique, `TotalSeconds`, `LastSeenAt`).

### Runtime services

- **`EventSubWebSocketHostedService`** - connects to `wss://eventsub.wss.twitch.tv/ws`, reconnects with exponential backoff (2 s → 2 min), dispatches notifications to registered `IEventSubHandler`s.
- **`ChannelChatMessageHandler`** - subscribes to `channel.chat.message` v1, parses `!name args` and routes to an `IChatCommand` via `ChatCommandDispatcher`. Replies are sent through `ChatResponder` using the bot's refreshed token.
- **`WatchtimePoller`** - every 60 s (configurable via `Watchtime:Interval`), checks whether the channel is live and, if so, lists chatters and bumps their watchtime.
- **`BotStatusTracker` / `StreamStatsTracker`** - singletons that expose live connection state, viewer count and 1-hour viewer delta to the UI.

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
    "BotUserId": "<twitch-user-id-of-the-bot-account>",
    "BroadcasterId": "<twitch-user-id-of-the-broadcaster>"
  },
  "Watchtime": {
    "Interval": "00:01:00"
  },
  "EventSub": {
    "Url": "wss://eventsub.wss.twitch.tv/ws"
  }
}
```

- `ConnectionStrings:AshryverBot` - Npgsql connection string.
- `DataProtection:KeyPath` - when set, ASP.NET Data Protection keys are persisted to that directory (required for stable cookie/token encryption across restarts).
- `Twitch:ClientId` / `Twitch:ClientSecret` - credentials of your Twitch application.
- `Twitch:BotUserId` - Twitch user id of the account the bot runs as.
- `Twitch:BroadcasterId` - Twitch user id of the channel the bot watches. The watchtime poller and the EventSub chat subscription disable themselves with a warning if this is missing.
- `Watchtime:Interval` - optional, overrides the 60-second poll interval.
- `EventSub:Url` - optional, overrides the WebSocket endpoint (useful for the Twitch CLI mock server).

Register a Twitch application at <https://dev.twitch.tv/console/apps>. The OAuth callback Blazor uses by default is `https://<host>/signin-twitch`.

### Bot account prerequisites

For chat ingest and `!watchtime` to work:

1. The bot account must have logged in at least once via `/login` so its OAuth token is persisted (matched against `Twitch:BotUserId`, stored with `IsBotAccount=true`).
2. The bot account must be a moderator of the broadcaster (required for the `moderator:read:chatters` scope and the `channel.chat.message` subscription).
3. `Twitch:BroadcasterId` must be set.

The bot already requests `user:read:chat`, `user:write:chat` and `moderator:read:chatters` as part of `TwitchScopes.All`.

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
- Twitch credentials are **not** baked into the compose file - provide them via `appsettings.local.json` (which the prod Dockerfile excludes from the image), environment variables, or your own override.

### Without Docker

1. Start a PostgreSQL instance and put the connection string in `appsettings.local.json`.
2. Fill in the `Twitch:*` keys.
3. Run the web app:

```bash
cd src
dotnet run --project AshryverBot.Web
```

Migrations are applied automatically on startup (`db.Database.MigrateAsync()` in `Program.cs`).

### Running the tests

```bash
cd src
dotnet test
```

## Database migrations

Migrations live in `AshryverBot.Database/Migrations` (currently: `Initial`, `AddCommands`, `TwitchTokenGuidPk`, `AddWatchtime`). To add a new one:

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