using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestSolution.Interfaces;

namespace TestSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _rateService;

        public ExchangeRateController(IExchangeRateService rateService)
        {
            _rateService = rateService;
        }
        
        /// <summary>
        /// Получить курс
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            try
            {
                var rate = await _rateService.GetExchangeRate();
                return Ok(rate);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}