using Microsoft.AspNetCore.Mvc;
using TradingManBackend.BusinessLayer.Logic;
using TradingManBackend.PresentationLayer.Dtos;

namespace TradingManBackend.PresentationLayer.Controllers
{
    /// <summary>
    /// Controller class responsible for handling REST API calls from frontend related to
    /// processing user given orders.
    /// </summary>
    [ApiController]
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly OrderLogic _tradeLogic;
        private readonly ILogger<OrdersController> _logger;  

        public OrdersController(OrderLogic tradeLogic, ILogger<OrdersController> logger)
        {
            _tradeLogic = tradeLogic;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to place an order with a broker.
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
        {
            _logger.LogInformation($"Placing an order for userId: [{orderDto.UserId}]");
            try
            {
                await _tradeLogic.PlaceOrder(OrderDtoHelper.FromOrderDto(orderDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
