namespace CryptoLinker.Core.Models
{
    public class Trade
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public long Time { get; set; }
        public bool IsBuyerMaker { get; set; }
    }
}
