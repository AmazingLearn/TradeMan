using Alpaca.Markets;
using TradingManBackend.DataLayer.ExternalAPI;
using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.BusinessLayer.Logic.Broker
{
    /// <summary>
    /// Implementation of Alpaca broker logic
    /// </summary>
    public class AlpacaLogic : IBrokerLogic
    {
        AlpacaApi _alpacaApi = new AlpacaApi();

        public async Task<IEnumerable<Product>> GetAllAvailableProducts(string exchange, AccountSettings accountSettings)
        {
            var apExchange = GetExhange(exchange);
            var assets = await _alpacaApi.GetAllAssets(apExchange, accountSettings);
            return assets.Select(x => new Product(x.Symbol, x.Name));
        }

        public async Task<bool> SubmitTrade(Order order, AccountSettings accountSettings)
        {
            decimal quantity = (decimal)order.OrderVolume;
            var orderQuantity = GetOrderQuantity(order.OrderVolumeType, quantity);
            return await _alpacaApi.SubmitTrade(order, accountSettings, orderQuantity);
        }

        public async Task<bool> Validate(AccountSettings accountSettings)
        {
            return await _alpacaApi.Validate(accountSettings);
        }

        private Exchange GetExhange(string exchange)
        {
            switch (exchange)
            {
                case "nysemkt":
                    return Exchange.NyseMkt;
                case "nysearca":
                    return Exchange.NyseArca;
                case "nyse":
                    return Exchange.Nyse;
                case "nasdaq":
                    return Exchange.Nasdaq;
                case "bats":
                    return Exchange.Bats;
                case "amex":
                    return Exchange.Amex;
                case "arca":
                    return Exchange.Arca;
                case "iex":
                    return Exchange.Iex;
                case "otc":
                    return Exchange.Otc;
            }
            
            return Exchange.Unknown;
        }

        private OrderQuantity GetOrderQuantity(OrderVolumeType orderVolumeType, decimal quantity)
        {
            if (orderVolumeType == OrderVolumeType.Shares)
            {
                return OrderQuantity.Fractional(quantity);
            }
                
            return OrderQuantity.Notional(quantity);
        }
    }
}
