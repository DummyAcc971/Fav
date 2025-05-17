using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockWatchlist.Models;
using StockWatchlist.Services;

namespace StockWatchlist.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IStockService _stockService;

        public StockRepository(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<List<StockData>> GetStockQuoteAsync(List<string> symbols)
        {
            Console.WriteLine($"Fetching live stock data for: {string.Join(", ", symbols)}");

            if (symbols == null || symbols.Count == 0)
            {
                Console.WriteLine("No symbols received by repository.");
                return new List<StockData>();
            }

            return await _stockService.FetchStockDataForMultipleSymbolsAsync(symbols);
        }
    }
}
