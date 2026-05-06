using AshryverBot.Twitch.Clients.Interfaces;
using Microsoft.Extensions.Logging;

namespace AshryverBot.Twitch.Clients;

public class TwitchClient(HttpClient httpClient, ILogger<TwitchClient> logger) : ITwitchClient
{

}