using System.Collections.Generic;
using System.Threading.Tasks;
using GuestAPI.Models;

namespace GuestAPI.Repositories
{
    public interface IGuestStockSymbolRepository
    {
        Task<List<StockSymbol>> GetGuestStockSymbolsAsync();
    }
}
