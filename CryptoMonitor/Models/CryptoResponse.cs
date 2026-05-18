using System.Text.Json.Serialization;

namespace CryptoMonitor.Models
{
    public class CryptoData
    {
        [JsonPropertyName("usd")]
        public decimal Usd { get; set; }
    }

    public class CryptoResponse
    {
        [JsonPropertyName("bitcoin")]
        public CryptoData? Bitcoin { get; set; }

        [JsonPropertyName("ethereum")]
        public CryptoData? Ethereum { get; set; }

        [JsonPropertyName("solana")]
        public CryptoData? Solana { get; set; }
    }
}
