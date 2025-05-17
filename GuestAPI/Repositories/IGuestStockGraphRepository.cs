using GuestAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuestAPI.Repositories
{
    public interface IGuestStockGraphRepository
    {
        Task<IEnumerable<StockDataPoint>> GetStockDataAsync(string symbol);
    }
}
