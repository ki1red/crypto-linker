using System.Text.Json.Serialization;

namespace CryptoLinker.Core.Models
{
    public class Ticker
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("price")]
        [JsonConverter(typeof(JsonStringToDecimalConverter))] // значение приходит строковым а не числовым
        public decimal Price { get; set; }
    }
}
