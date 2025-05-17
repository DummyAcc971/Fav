using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StockWatchlist.Repositories;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace StockWatchlistAPI.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _repository;
        private readonly IConfiguration _configuration;

        public StockController(IStockRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> GetStocks([FromQuery] string symbols)
        {
            try
            {
                string apiKey = _configuration["TwelveData:ApiKey"];
                Console.WriteLine("Fetched API Key in Controller: " + apiKey);

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new UnauthorizedAccessException("API Key is missing from configuration.");
                }

                Console.WriteLine("Received API Request for Symbols: " + symbols);

                if (string.IsNullOrWhiteSpace(symbols))
                {
                    throw new ArgumentException("Symbols parameter is required.");
                }

                var symbolList = symbols.Split(',').ToList();
                Console.WriteLine("Parsed Symbols List: " + string.Join(", ", symbolList));

                var result = await _repository.GetStockQuoteAsync(symbolList);

                if (result == null || result.Count == 0)
                {
                    throw new InvalidOperationException("No stock data found.");
                }

                Console.WriteLine("StockController Response Count: " + result.Count);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Unauthorized Access: " + ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid Argument: " + ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("External API Error: " + ex.Message);
                return StatusCode(502, new { message = "Error communicating with external API." });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Operation Error: " + ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected Error: " + ex.Message);
                return StatusCode(500, new { message = "An unexpected error occurred: " + ex.Message });
            }
        }
    }
}
