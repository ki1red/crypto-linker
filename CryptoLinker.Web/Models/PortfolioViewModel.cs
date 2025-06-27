using CryptoLinker.Core.Models;

namespace CryptoLinker.Web.Models
{
    public class PortfolioViewModel
    {
        public List<PortfolioItem> Items { get; set; } = new();
        public Dictionary<string, Dictionary<string, decimal>> Calculated { get; set; }
    }
}
