using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CryptoLinker.Core.Services
{
    public class BinanceStatusService
    {
        private const string BaseUrl = "https://api.binance.com";
        private const string ForPing = "/api/v3/ping";
        private const string ForTestTime = "api/v3/time";

        private readonly HttpClient _httpClient;

        public BinanceStatusService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        // Считаем время доступности до бинанса
        public async Task<long> GetServerTimeAsync()
        {
            var response = await _httpClient.GetAsync(ForTestTime);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("serverTime").GetInt64();
        }

        // Проверяем доступность бинанса
        public async Task<bool> PingAsync()
        {
            var response = await _httpClient.GetAsync(ForPing);
            return response.IsSuccessStatusCode;
        }
    }
}
