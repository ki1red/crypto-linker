using System.Text.Json.Serialization;

namespace CryptoLinker.Core.Models
{
    public class Trade
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("time")]
        public long Time { get; set; }
        [JsonPropertyName("isBuyerMaker")]
        public bool IsBuyerMaker { get; set; }
    }
}
