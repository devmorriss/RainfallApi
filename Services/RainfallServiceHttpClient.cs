using RainfallApi.Contracts;

namespace RainfallApi.Services;

public class RainfallServiceHttpClient
{
    private readonly HttpClient _client;
    private readonly IConfiguration _config;

    public RainfallServiceHttpClient(HttpClient client, IConfiguration config)
    {
        _client = client;
        _config = config;
    }

    public async Task<RainfallApiBaseResponse> GetRainfallApiReadings(int stationId, int? limit)
    {
        var rainfallUrl = _config["RainfallApiUrl"] + "/id/stations/" + stationId + "/readings";

        if (limit.HasValue)
            rainfallUrl += $"?_limit={limit}";

        Console.WriteLine(rainfallUrl);

        return await _client.GetFromJsonAsync<RainfallApiBaseResponse>(rainfallUrl);
    }
}
