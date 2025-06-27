using CryptoLinker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLinker.Core.Services
{
    public class PortfolioCalculator
    {
        private readonly RialtoMarketClient _marketClient;
        private readonly List<PortfolioItem> _portfolioItems;

        private readonly string[] _targetCurrencies = new[] { "USDT", "BTC", "XRP", "XMR", "DASH" };

        public PortfolioCalculator(List<PortfolioItem> portfolioItems)
        {
            _marketClient = new RialtoMarketClient();
            _portfolioItems = portfolioItems;
        }

        public async Task<Dictionary<string, Dictionary<string, decimal>>> CalculateAsync()
        {
            var result = new Dictionary<string, Dictionary<string, decimal>>();
            foreach (var item in _portfolioItems.Where(p => _targetCurrencies.Contains(p.Symbol)))
            {
                var conversions = new Dictionary<string, decimal>();

                foreach (var target in _targetCurrencies)
                {
                    if (item.Symbol == target)
                    {
                        conversions[target] = item.Amount;
                        continue;
                    }

                    string pair = item.Symbol + target;
                    string inversePair = target + item.Symbol;
                    decimal rate;

                    try
                    {
                        var ticker = await _marketClient.GetTickerAsync(pair);
                        rate = ticker.Price;
                    }
                    catch (HttpRequestException)// TODO нужно ли чекать искл
                    {
                        try
                        {
                            var ticker = await _marketClient.GetTickerAsync(inversePair);
                            rate = 1 / ticker.Price;
                        }
                        catch (HttpRequestException) // TODO нужно ли чекать искл
                        {
                            continue;
                        }
                    }
                    conversions[target] = item.Amount * rate;
                }
                result[item.Symbol] = conversions;
            }
            return result;
        }
    }
}
