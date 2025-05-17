using GuestAPI.Models;
using System.Threading.Tasks;

namespace GuestAPI.Services
{
    public interface IGuestStockService
    {
        Task<StockQuote?> GetGuestStockDataAsync(string symbol);
    }
}
