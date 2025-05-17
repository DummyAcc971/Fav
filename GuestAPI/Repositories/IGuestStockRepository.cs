using GuestAPI.Models;
using System.Threading.Tasks;

namespace GuestAPI.Repositories
{
    public interface IGuestStockRepository
    {
        Task<StockQuote?> GetGuestStockQuoteAsync(string symbol);
    }
}
