namespace GuestAPI.Models
{
    public class StockSymbol
    {
        public string Symbol { get; set; }
        public string Description { get; set; }

        public StockSymbol()
        {
            Symbol = string.Empty;
            Description = string.Empty;
        }
    }
}
