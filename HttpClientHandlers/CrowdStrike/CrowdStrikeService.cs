using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HttpClientHandlers.CrowdStrike;

public class CrowdStrikeService : ICrowdStrikeService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;
    private string? _accessToken;
    private DateTime _tokenExpiration;

    public CrowdStrikeService(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _configuration = configuration;
    }


    private async Task<string?> GetAccessTokenAsync()
    {
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiration)
        {
            return _accessToken;
        }

        var clientId = _configuration["ApiAccessConfigs:CrowdStrike:ClientId"];
        var clientSecret = _configuration["ApiAccessConfigs:CrowdStrike:ClientSecret"];
        var baseUrl = _configuration["ApiAccessConfigs:CrowdStrike:BaseUrl"];
        var client = _clientFactory.CreateClient("CrowdStrike");

        // Encode ClientId and Secret into a Base64 string
        var bytes = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
        var base64 = Convert.ToBase64String(bytes);

        // Set the Authorization header
        var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

        // Prepare the request body
        var oauthRequestBody = new StringContent("grant_type=client_credentials", Encoding.UTF8,
            "application/x-www-form-urlencoded");

        // Make the request
        var response = await client.PostAsync($"{baseUrl}/oauth2/token", oauthRequestBody);

        if (!response.IsSuccessStatusCode) throw new Exception("Could not retrieve access token.");
        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
        if (tokenResponse == null)
        {
            throw new Exception("Failed to deserialize the token response.");
        }

        _accessToken = tokenResponse!["access_token"]?.ToString();

        _tokenExpiration = GetTokenExpiration(_accessToken);

        return _accessToken;
    }

    private static DateTime GetTokenExpiration(string? accessToken)
    {
        var tokenParts = accessToken.Split('.');
        if (tokenParts.Length < 2)
        {
            throw new ArgumentException("Invalid access token format.");
        }

        var payload = tokenParts[1];
        payload = payload.Replace('-', '+').Replace('_', '/');
        var payloadBytes =
            Convert.FromBase64String(payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '='));
        var decodedPayload = Encoding.UTF8.GetString(payloadBytes);
        var tokenData = JsonSerializer.Deserialize<Dictionary<string, object>>(decodedPayload);
        if (tokenData == null || !tokenData.TryGetValue("exp", out var value) ||
            !int.TryParse(value.ToString(), out var result))
            throw new ArgumentException("Expiration time not found in access token.");
        var offset = DateTimeOffset.FromUnixTimeSeconds(result).ToUniversalTime();
        return offset.DateTime;
    }
    public async Task<CrowdStrikeToken?> GetAccessToken()
    {
        var accessToken = await GetAccessTokenAsync();
        return new CrowdStrikeToken()
        {
            AccessToken = accessToken,
            ExpirationUtc = _tokenExpiration
        };
    }
    private async Task<List<string>> GetDeviceIdsAsync(string filter)
    {
        var accessToken = await GetAccessTokenAsync();
        var client = _clientFactory.CreateClient("CrowdStrike");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var url = $"{_configuration["ApiAccessConfigs:CrowdStrike:BaseUrl"]}/devices/queries/devices-scroll/v1{filter}";
        Console.WriteLine(url);
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<CsDeviceIdResponse>(content);
            var deviceIds = jsonResponse.resources;

            return deviceIds ?? new List<string>();
            
        }

        throw new Exception($"Could not retrieve device IDs. Response: {response.StatusCode}, {response.ReasonPhrase}");
    }
    public async Task<List<CsDeviceDetailResource>> GetDeviceDetailsAsync(List<string> deviceIds)
    {
        const int batchSize = 5000;
        var accessToken = await GetAccessTokenAsync();
        var client = _clientFactory.CreateClient("CrowdStrike");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var allDevices = new List<CsDeviceDetailResource>();

        for (int i = 0; i < deviceIds.Count; i += batchSize)
        {
            var batchIds = deviceIds.Skip(i).Take(batchSize);
            //var url = $"{_configuration["ApiAccessConfigs:CrowdStrike:BaseUrl"]}/devices/entities/devices/v1/?ids={string.Join("&ids=", batchIds)}";
            var url = $"{_configuration["ApiAccessConfigs:CrowdStrike:BaseUrl"]}/devices/entities/devices/v2";
            var body = new{ ids = batchIds.ToArray() };
            var bodyString = JsonSerializer.Serialize(body);
            var buffer = System.Text.Encoding.UTF8.GetBytes(bodyString);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(url,byteContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<CsDeviceDetail>(content);
                var devices = jsonResponse?.Resources?? []; // ?? new List<CrowdStrikeDeviceResponse>();

                allDevices.AddRange(devices);
            }
            else
            {
                throw new Exception($"Could not retrieve device details. Response: {response.StatusCode}, {response.ReasonPhrase}");
            }
        }

        return allDevices;
    }
    public async Task<string> GetHostNameInfoAsync(string hostname)
    {
        var filter = $"?filter=hostname:'{hostname}'";
        var devices = await GetDeviceIdsAsync(filter);
        return "";

    }




    public async Task<List<CsDeviceDetailResource>?> GetStatusForHostname(string hostname)
    {
        var filter = $"?filter=hostname:'{hostname}'";
        var devices = await GetDeviceIdsAsync(filter);
        if (devices.Count > 0) 
            return await GetDeviceDetailsAsync(devices);
        return [];
    }
}