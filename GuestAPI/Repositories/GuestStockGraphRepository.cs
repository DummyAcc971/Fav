using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using GuestAPI.Models;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuestAPI.Repositories
{
    public class GuestStockGraphRepository : IGuestStockGraphRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GuestStockGraphRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:TwelveData"] ?? throw new ArgumentNullException("API Key is missing in configuration.");
        }

        public async Task<IEnumerable<StockDataPoint>> GetStockDataAsync(string symbol)
        {
            string url = $"https://api.twelvedata.com/time_series?symbol={symbol}&interval=1min&outputsize=1440&apikey={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var values = json["values"]?.ToList();

            if (values == null || values.Count == 0)
            {
                throw new Exception("No data available for this symbol or invalid symbol.");
            }

            var stockData = values
                .Select(item => new StockDataPoint
                {
                    Datetime = item["datetime"]?.ToString(),
                    Price = double.Parse(item["close"]?.ToString() ?? "0", CultureInfo.InvariantCulture)
                })
                .ToList();

            return stockData;
        }
    }
}
