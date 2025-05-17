using GuestAPI.Repositories;
using GuestAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuestAPI.Services
{
    public class GuestStockSymbolService : IGuestStockSymbolService
    {
        private readonly IGuestStockSymbolRepository _repository;

        public GuestStockSymbolService(IGuestStockSymbolRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StockSymbol>> GetGuestStockSuggestionsAsync(string query)
        {
            var stockSymbols = await _repository.GetGuestStockSymbolsAsync();

            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                return new List<StockSymbol>(); // Ensure at least 2 characters for meaningful search
            }

            return stockSymbols
                .Where(s => s.Symbol.StartsWith(query, System.StringComparison.OrdinalIgnoreCase) ||
                            s.Description.StartsWith(query, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
