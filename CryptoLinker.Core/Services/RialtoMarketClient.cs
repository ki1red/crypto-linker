using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CryptoLinker.Core.Models;

namespace CryptoLinker.Core.Services
{
    public class RialtoMarketClient
    {
        private const string BaseUrl = "https://api.binance.com";
        private const string TickerPriceEndpoint = "/api/v3/ticker/price";
        private const string TradesEndpoint = "api/v3/trades";
        private const string KlinesEndpoint = "api/v3/klines";

        private readonly HttpClient _httpClient;

        public RialtoMarketClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        // Получение текущей цены тикера
        public async Task<Ticker> GetTickerAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"{TickerPriceEndpoint}?symbol={symbol}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Ticker>(json);
        }

        // Получение последних трейдов по символу
        public async Task<List<Trade>> GetTradesAsync(string symbol, int limit = 10)
        {
            var response = await _httpClient.GetAsync($"{TradesEndpoint}?symbol={symbol}&limit={limit}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Trade>>(json);
        }

        //
        public async Task<List<Candle>> GetCandleAsync(string symbol, string interval, int limit = 10)
        {
            var response = await _httpClient.GetAsync($"{KlinesEndpoint}?symbol={symbol}&interval={interval}&linit={limit}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var rawArray = JsonSerializer.Deserialize<List<List<object>>>(json);

            var candles = new List<Candle>();
            foreach (var entry in rawArray)
            {
                candles.Add(new Candle
                {
                    OpenTime = (long)Convert.ToDouble(entry[0]),
                    Open = decimal.Parse(entry[1].ToString()),
                    High = decimal.Parse(entry[2].ToString()),
                    Low = decimal.Parse(entry[3].ToString()),
                    Close = decimal.Parse(entry[4].ToString()),
                    Volume = decimal.Parse(entry[5].ToString())
                });
            }
            return candles;
        }
    }
}
