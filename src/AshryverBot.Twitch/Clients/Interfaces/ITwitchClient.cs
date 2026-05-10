namespace AshryverBot.Twitch.Clients.Interfaces;

public interface ITwitchClient
{
    Task<TResponse> GetAsync<TResponse>(
        string resourceUrl,
        string accessToken,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);

    Task<TResponse> PostAsync<TRequest, TResponse>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);

    Task<TResponse> PostAsync<TResponse>(
        string resourceUrl,
        string accessToken,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);

    Task PostAsync(
        string resourceUrl,
        string accessToken,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);

    Task<TResponse> PatchAsync<TRequest, TResponse>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);

    Task PatchAsync<TRequest>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);

    Task<TResponse> PutAsync<TRequest, TResponse>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);

    Task PutAsync<TRequest>(
        string resourceUrl,
        string accessToken,
        TRequest payload,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string resourceUrl,
        string accessToken,
        IReadOnlyList<KeyValuePair<string, string>>? queryParameters = null,
        CancellationToken cancellationToken = default);
}
