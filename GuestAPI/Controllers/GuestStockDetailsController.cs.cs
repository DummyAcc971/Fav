using Microsoft.AspNetCore.Mvc;
using GuestAPI.Repositories;

namespace GuestAPI.Controllers
{
    [ApiController]
    [Route("api/gueststockdetails")]
    public class GuestStockDetailsController : ControllerBase
    {
        private readonly IGuestStockRepository _repository;

        public GuestStockDetailsController(IGuestStockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetGuestStockDetails(string symbol)
        {
            var result = await _repository.GetGuestStockQuoteAsync(symbol);
            if (result == null)
                return Unauthorized("Signup required after 2 searches.");

            return Ok(result);
        }
    }
}
