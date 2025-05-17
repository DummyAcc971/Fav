namespace StockWatchlist.Models
{
    public class StockData
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PercentChange { get; set; }

        public StockData(string symbol, string name, decimal price, decimal percentChange)
        {
            Symbol = symbol;
            Name = name;
            Price = price;
            PercentChange = percentChange;
        }
    }
}
