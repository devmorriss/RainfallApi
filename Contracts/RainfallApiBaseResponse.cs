using System.Text.Json.Serialization;

namespace RainfallApi.Contracts;

public class RainfallApiBaseResponse
{
    [JsonPropertyName("@context")]
    public string Context { get; set; }
    public Meta Meta { get; set; }
    public List<Item> Items { get; set; }
}
