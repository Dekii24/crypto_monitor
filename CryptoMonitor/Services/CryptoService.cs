using CryptoMonitor.Models;
using System.Text.Json;

namespace CryptoMonitor.Services
{
    public class CryptoService
    {
        private readonly HttpClient _httpClient;

        public CryptoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CryptoResponse?> GetPricesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum&vs_currencies=usd");
                response.EnsureSuccessStatusCode();

                var jsonData = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<CryptoResponse>(jsonData);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while retrieving data: {e.Message}");
                return null;
            }
        }
    }
}
