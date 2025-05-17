using GuestAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuestAPI.Services
{
    public interface IGuestStockGraphService
    {
        Task<IEnumerable<StockDataPoint>> GetStockDataAsync(string symbol);
    }
}
