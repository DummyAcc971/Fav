using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using GuestAPI.Models;

namespace GuestAPI.Repositories
{
    public class GuestStockSymbolRepository : IGuestStockSymbolRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "GuestStockSymbols";

        public GuestStockSymbolRepository(HttpClient httpClient, IMemoryCache cache, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<List<StockSymbol>> GetGuestStockSymbolsAsync()
        {
            var baseUrl = _configuration["Finnhub:BaseUrl"];
            var apiKey = _configuration["Finnhub:ApiKey"];

            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("Finnhub API BaseUrl or ApiKey is missing from configuration.");
            }

            var requestUrl = $"{baseUrl}stock/symbol?exchange=US&token={apiKey}";

            if (_cache.TryGetValue(CacheKey, out List<StockSymbol> cachedSymbols))
            {
                return cachedSymbols;
            }

            using var responseStream = await _httpClient.GetStreamAsync(requestUrl);
            var asyncEnumerable = JsonSerializer.DeserializeAsyncEnumerable<StockSymbol>(
                responseStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var symbols = new List<StockSymbol>();
            await foreach (var stock in asyncEnumerable)
            {
                if (stock != null)
                {
                    symbols.Add(stock);
                }
            }

            _cache.Set(CacheKey, symbols, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4)
            });

            return symbols ?? new List<StockSymbol>();
        }
    }
}
