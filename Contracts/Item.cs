using System.Text.Json.Serialization;

namespace RainfallApi.Contracts;

public class Item
{
    [JsonPropertyName("@id")]
    public string Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Measure { get; set; }

    private decimal _value;

    public decimal Value
    {
        get { return _value; }
        set
        {
            _value = Math.Round(value, 1);
        }
    }
}
