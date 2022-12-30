using Microsoft.AspNetCore.Mvc;
using TradingManBackend.BusinessLayer.Logic;
using TradingManBackend.PresentationLayer.Dtos;

namespace TradingManBackend.PresentationLayer.Controllers
{
    /// <summary>
    /// Controller class responsible for handling REST API calls from frontend related to
    /// proposed positions.
    /// </summary>
    [ApiController]
    [Route("Positions")]
    public class PositionsController : Controller
    {
        private readonly PositionLogic _positionLogic;
        private readonly StockDataLogic _stockDataLogic;
        private readonly ILogger<PositionsController> _logger;

        public PositionsController(PositionLogic positionLogic, StockDataLogic stockDataLogic, ILogger<PositionsController> logger)
        {
            _positionLogic = positionLogic;
            _stockDataLogic = stockDataLogic;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint that returns all proposed positions for selected userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllPositions/{userId}")]
        public async Task<IActionResult> GetAllPositions([FromRoute] Guid userId)
        {
            _logger.LogInformation($"Getting all proposed possitions for userId: [{userId}].");
            List<PositionDto> positionDtos = new List<PositionDto>();
            try
            {
                var positions = _positionLogic.GetAllPositions(userId);
                    
                foreach (var position in positions)
                {
                    var curentPrice = await _stockDataLogic.GetCurrentPrice(position.ProductSymbol);
                    positionDtos.Add(PositionsDtoHelper.ToPositionDto(position, curentPrice));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(positionDtos);
        }

        /// <summary>
        /// Endpoint that returns proposed position based on userId and positionId - for displaying positions directly
        /// from a link sent to users.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPosition/{userId}/{positionId}")]
        public async Task<IActionResult> GetPositionAsync([FromRoute] Guid userId, [FromRoute] Guid positionId)
        {
            _logger.LogInformation($"Retrieving information about position with userId: [{userId}] and positionId [{positionId}].");
            PositionDto positionDto;
            try
            {
                var position = _positionLogic.GetPosition(userId, positionId);
                var currentPrice = await _stockDataLogic.GetCurrentPrice(position.ProductSymbol);

                positionDto = PositionsDtoHelper.ToPositionDto(position, currentPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(positionDto);
        }

        /// <summary>
        /// Endpoint for removing unwanted proposed posiiton
        /// </summary>
        /// <param name="positionDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemovePisition/{positionId}")]
        public IActionResult RemovePosition([FromRoute] Guid positionId)
        {
            _logger.LogInformation($"Removing proposed position with positionId: [{positionId}].");
            try
            {
                _positionLogic.RemovePosition(positionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Ok();
        }
    }
}
