using System.Text.Json;
using GuestAPI.Services;
using GuestAPI.Models;

namespace GuestAPI.Services
{
    public class GuestStockService : IGuestStockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GuestStockService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<StockQuote?> GetGuestStockDataAsync(string symbol)
{
    var baseUrl = _configuration["Finnhub:BaseUrl"];
    var apiKey = _configuration["Finnhub:ApiKey"];

    if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiKey))
    {
        throw new InvalidOperationException("Finnhub API base URL or API key is missing from configuration.");
    }

    using var client = _httpClientFactory.CreateClient();
    client.BaseAddress = new Uri(baseUrl); // Fix missing base URL

    var response = await client.GetAsync($"quote?symbol={symbol}&token={apiKey}");
    
    if (!response.IsSuccessStatusCode)
    {
        throw new HttpRequestException($"Failed request: {response.StatusCode}");
    }

    var json = await response.Content.ReadFromJsonAsync<JsonElement>();

    return new StockQuote(symbol)
    {
        CurrentPrice = (decimal)json.GetProperty("c").GetDouble(),
        Open = (decimal)json.GetProperty("o").GetDouble(),
        High = (decimal)json.GetProperty("h").GetDouble(),
        Low = (decimal)json.GetProperty("l").GetDouble(),
        PreviousClose = (decimal)json.GetProperty("pc").GetDouble(),
    };
}

    }
}
