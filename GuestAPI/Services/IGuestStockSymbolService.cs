using GuestAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuestAPI.Services
{
    public interface IGuestStockSymbolService
    {
        Task<List<StockSymbol>> GetGuestStockSuggestionsAsync(string query);
    }
}
