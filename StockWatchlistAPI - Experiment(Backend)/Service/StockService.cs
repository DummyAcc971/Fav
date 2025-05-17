using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StockWatchlist.Models;

namespace StockWatchlist.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public StockService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TwelveData:ApiKey"];
            _baseUrl = configuration["TwelveData:BaseUrl"];

            if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_baseUrl))
            {
                throw new ArgumentNullException("API key or Base URL is missing.");
            }

            Console.WriteLine("Loaded API Key: " + _apiKey);
            Console.WriteLine("Loaded Base URL: " + _baseUrl);
        }

        public async Task<List<StockData>> FetchStockDataForMultipleSymbolsAsync(List<string> stockSymbols)
        {
            try
            {
                Console.WriteLine("Fetching stock data for symbols: " + string.Join(", ", stockSymbols));

                if (stockSymbols == null || stockSymbols.Count == 0)
                {
                    throw new ArgumentException("Stock symbols list is empty.");
                }

                string symbolsQuery = string.Join(",", stockSymbols);
                string url = _baseUrl + "/quote?symbol=" + symbolsQuery + "&apikey=" + _apiKey;

                Console.WriteLine("Calling API: " + url);

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Response: " + responseBody);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("API call failed with status: " + response.StatusCode);
                }

                JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                List<StockData> stockList = new List<StockData>();

                foreach (var property in jsonDocument.RootElement.EnumerateObject())
                {
                    Console.WriteLine("Processing stock: " + property.Name);

                    JsonElement stockElement = property.Value;

                    decimal closePrice = stockElement.TryGetProperty("close", out var closeProp) &&
                                         decimal.TryParse(closeProp.GetString(), out var parsedClose)
                        ? parsedClose
                        : 0;

                    decimal percentChange = stockElement.TryGetProperty("percent_change", out var changeProp) &&
                                            decimal.TryParse(changeProp.GetString(), out var parsedChange)
                        ? parsedChange
                        : 0;

                    stockList.Add(new StockData(
                        stockElement.GetProperty("symbol").GetString() ?? "N/A",
                        stockElement.GetProperty("name").GetString() ?? "Unknown",
                        closePrice,
                        percentChange
                    ));
                }

                Console.WriteLine("Parsed Stock List: " + stockList.Count + " items fetched.");
                return stockList;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid Argument: " + ex.Message);
                return new List<StockData> { new StockData("Error", "Invalid input", 0, 0) };
            }
            catch (JsonException ex)
            {
                Console.WriteLine("JSON Parsing Error: " + ex.Message);
                return new List<StockData> { new StockData("Error", "Failed to parse API response", 0, 0) };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("API Request Error: " + ex.Message);
                return new List<StockData> { new StockData("Error", "External API failure", 0, 0) };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected Error: " + ex.Message);
                return new List<StockData> { new StockData("Error", "An unexpected error occurred", 0, 0) };
            }
        }
    }
}
