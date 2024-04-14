using System.Net;
using System.Web;
using Microsoft.Extensions.Caching.Memory;

namespace HttpClientHandlers.Weather;

public class CachedWeatherHandler : DelegatingHandler
{
    private readonly IMemoryCache _cache;

    public CachedWeatherHandler(IMemoryCache cache)
    {
        _cache = cache;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var queryString = HttpUtility.ParseQueryString(request.RequestUri!.Query);
        var query = queryString["q"];
        var units = queryString["units"];
        var key = $"{query}-{units}";
        var cached = _cache.Get<string>(key);
        if (cached is not null)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(cached)
            };
        }
        
        var response = await base.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        _cache.Set(key, content, TimeSpan.FromMinutes(1));
        return response;
    }
}



