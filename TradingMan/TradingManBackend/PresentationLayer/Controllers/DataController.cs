using Microsoft.AspNetCore.Mvc;
using TradingManBackend.BusinessLayer.Logic;
using TradingManBackend.BusinessLayer.Logic.Broker;
using TradingManBackend.DataLayer.Models;
using TradingManBackend.PresentationLayer.Dtos;

namespace TradingManBackend.PresentationLayer.Controllers
{
    /// <summary>
    /// Controller class responsible for handling REST API calls from frontend related to
    /// stock data.
    /// </summary>
    [ApiController]
    [Route("Data")]
    public class DataController : Controller
    {
        private readonly StockDataLogic _dataLogic;
        private readonly IBrokerLogic _brokerLogic;
        private readonly UsersLogic _usersLogic;
        private readonly ILogger<DataController> _logger;

        public DataController(
            StockDataLogic dataLogic,
            ILogger<DataController> logger,
            AlpacaLogic alpacaLogic,
            UsersLogic usersLogic
            )
        {
            _dataLogic = dataLogic;
            _logger = logger;
            _brokerLogic = alpacaLogic;
            _usersLogic = usersLogic;
        }

        /// <summary>
        /// Endpoint returns available end of day data in time series form for requested pruct symbol.
        /// </summary>
        /// <param name="symbol">Ticker symbol of product</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDailyData/{symbol}")]
        public async Task<IActionResult> GetDailyData([FromRoute] string symbol)
        {
            _logger.LogInformation($"Gettind end of day data for symbol: [{symbol}]");
            var data = new List<StockData>();
            try
            {
                data = await _dataLogic.GetDailyData(symbol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(data.Select(x => StockDataDtoHelper.ToStockDataDto(x)));
        }

        /// <summary>
        /// Endpoint that returns all available products for given exhange.
        /// Requires user to be logged in.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="exchange"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProducts/{userId}/{exchange}")]
        public async Task<IActionResult> GetAllProducts([FromRoute] Guid userId, [FromRoute] string exchange)
        {
            _logger.LogInformation($"Getting all available products for exchange: [{exchange}].");
            var productDtos = new List<ProductDto>();
            try
            {
                var accountSettings = _usersLogic.GetAccountSettings(userId);
                var products = await _brokerLogic.GetAllAvailableProducts(exchange.ToLower(), accountSettings);
                productDtos = products.Select(x => ProductDtoHelper.ToProductDto(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(productDtos);
        }
    }
}
