using HttpClientHandlers.Weather;

namespace HttpClientHandlers.CrowdStrike;

public interface ICrowdStrikeService
{
    Task<CrowdStrikeToken?> GetAccessToken();
    Task<List<CsDeviceDetailResource>?> GetStatusForHostname(string hostname);
}
