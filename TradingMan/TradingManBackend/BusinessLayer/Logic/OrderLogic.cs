using TradingManBackend.BusinessLayer.Logic.Broker;
using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;


namespace TradingManBackend.BusinessLayer.Logic
{
    public class OrderLogic
    {
        // Using Alpaca as base, could be reworked to be decided at runtime inside brokerLogic.
        private readonly IBrokerLogic _brokerLogic = new AlpacaLogic();
        private readonly DatabaseContext _tmContext;
        private readonly ILogger<OrderLogic> _logger;

        public OrderLogic(DatabaseContext tmContext, ILogger<OrderLogic> logger)
        {
            _tmContext = tmContext;
            _logger = logger;
        }

        /// <summary>
        /// Submits a new trade with registerd broker.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<bool> PlaceOrder(Order order)
        {
            _logger.LogInformation($"Placing order for symbol: {order.ProductSymbol}");

            var accSettings = _tmContext.AccountSettings.Where(x => x.UserId == order.UserId).First();
            await _brokerLogic.SubmitTrade(order, accSettings);
            return true;
        }
    }
}
