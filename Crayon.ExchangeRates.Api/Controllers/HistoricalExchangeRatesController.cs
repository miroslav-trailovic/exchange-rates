using Crayon.ExchangeRates.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Crayon.ExchangeRates.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricalExchangeRatesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IHistoricalExchangeRatesBusinessLogic _historicalExchangeRatesBusinessLogic;
        
        public HistoricalExchangeRatesController(ILogger<HistoricalExchangeRatesController> logger,
            IHistoricalExchangeRatesBusinessLogic historicalExchangeRatesBusinessLogic)
        {
            _logger = logger;
            _historicalExchangeRatesBusinessLogic = historicalExchangeRatesBusinessLogic;
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery]string baseCurr, [FromQuery]string targetCurr, 
            [FromQuery]DateTime[] dates)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(await _historicalExchangeRatesBusinessLogic.
                    CalculateHistoricalExchangeRatesAsync(baseCurr, targetCurr, dates));
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex.Message);

                return BadRequest();
            }
        }
    }
}
