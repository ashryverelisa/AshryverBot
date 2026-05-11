using System.Net;
using System.Net.Http.Headers;

namespace AshryverBot.Twitch.Tests.TestSupport;

internal sealed class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

    public List<HttpRequestMessage> Requests { get; } = [];
    public List<string?> RequestBodies { get; } = [];

    public MockHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        _handler = handler;
    }

    public static MockHttpMessageHandler RespondWith(HttpStatusCode statusCode, string? json = null)
        => new(async (_, _) =>
        {
            var response = new HttpResponseMessage(statusCode);
            if (json is not null)
            {
                response.Content = new StringContent(json);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            return await Task.FromResult(response);
        });

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);

        string? body = null;
        if (request.Content is not null)
            body = await request.Content.ReadAsStringAsync(cancellationToken);
        RequestBodies.Add(body);

        return await _handler(request, cancellationToken);
    }
}
