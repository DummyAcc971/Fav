using Microsoft.AspNetCore.Mvc;
using GuestAPI.Services;
using GuestAPI.Models;
using System.Threading.Tasks;

namespace GuestAPI.Controllers
{
    [ApiController]
    [Route("api/gueststockgraph")]
    public class GuestStockGraphController : ControllerBase
    {
        private readonly IGuestStockGraphService _stockGraphService;

        public GuestStockGraphController(IGuestStockGraphService stockGraphService)
        {
            _stockGraphService = stockGraphService;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetStockGraphData(string symbol)
        {
            try
            {
                var stockData = await _stockGraphService.GetStockDataAsync(symbol);
                if (stockData == null || !stockData.Any())
                {
                    return NotFound("No data found for this symbol.");
                }
                return Ok(stockData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
