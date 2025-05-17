using System.Collections.Generic;
using System.Threading.Tasks;
using StockWatchlist.Models;

namespace StockWatchlist.Services
{
    public interface IStockService
    {
        Task<List<StockData>> FetchStockDataForMultipleSymbolsAsync(List<string> stockSymbols);
    }
}
