namespace CryptoLinker.Core.Models
{
    public class PortfolioItem
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        // TODO добавить возможность изменять количество каждой валюты
    }
}
