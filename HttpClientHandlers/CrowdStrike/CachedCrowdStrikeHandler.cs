using System.Net;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Caching.Memory;

namespace HttpClientHandlers.CrowdStrike;

public class CachedCrowdStrikeHandler : DelegatingHandler
{
    private readonly IMemoryCache _cache;

    public CachedCrowdStrikeHandler(IMemoryCache cache)
    {
        _cache = cache;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var key = await GenerateCacheKeyAsync(request);
        if (!string.IsNullOrEmpty(key) && _cache.TryGetValue(key, out string? cached))
        {
            Console.WriteLine("Cache hit");
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(cached??"")
            };
        }
        

        var response = await base.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        _cache.Set(key, content, TimeSpan.FromMinutes(1));
        return response;
    }
    private async Task<string> GenerateCacheKeyAsync(HttpRequestMessage request)
    {
        var requestUri = request.RequestUri?.AbsolutePath.Trim() ?? string.Empty;
        var queryString = HttpUtility.ParseQueryString(request.RequestUri?.Query ?? string.Empty);
//        var postIds = request.Method == HttpMethod.Post ? await GetPostRequestIdsAsync(request) : null;
        // if (requestUri == "/devices/entities/devices/v2")
        // {
        //     var postIds = await GetPostRequestIdsAsync(request);
        //     return $"detailV2-{postIds}";
        // }
        return requestUri.Trim('/') switch
        {
            "devices/queries/devices-scroll/v1" => $"ids-{queryString["filter"]}",
            "devices/entities/devices/v1" => $"detail-{queryString["ids"]}",
            "devices/entities/devices/v2" =>$"detailV2-{await GetPostRequestIdsAsync(request)}",
            _ => string.Empty
        };
    }
    private async Task<string> GetPostRequestIdsAsync(HttpRequestMessage request)
    {
        var body = await ReadRequestBodyAsync(request);
        var ids = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(body)?["ids"];
        return ids != null ? string.Join("-", ids) : string.Empty;
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequestMessage request)
    {
        request.Content!.Headers.ContentLength = null; // Resetting the Content-Length header

        var stream = await request.Content.ReadAsStreamAsync();
        var reader = new StreamReader(stream);
        var body = await reader.ReadToEndAsync();

        // Reset the stream so it can be read again later
        stream.Seek(0, SeekOrigin.Begin);
        request.Content = new StreamContent(stream);

        return body;
    }
}