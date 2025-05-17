using System.Collections.Generic;
using System.Threading.Tasks;
using StockWatchlist.Models;

namespace StockWatchlist.Repositories
{
    public interface IStockRepository
    {
        Task<List<StockData>> GetStockQuoteAsync(List<string> symbols);
    }
}
