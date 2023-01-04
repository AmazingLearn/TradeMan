using Alpaca.Markets;
using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.DataLayer.ExternalAPI
{
    /// <summary>
    /// Class containing methods of comunication with Alpaca API.
    /// Alpaca API is used to send market orders.
    /// </summary>
    public class AlpacaApi
    {
        /// <summary>
        /// Returns all available products - assests in Alapaca - for given exhcange.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="accountSettings"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<IAsset>> GetAllAssets(Exchange exchange, AccountSettings accountSettings)
        {
            var client = GetAlpacaTradingClient(accountSettings.AlpacaApiKey, accountSettings.AlpacaSecretKey, true);
            return await client.ListAssetsAsync(new AssetsRequest { AssetStatus = AssetStatus.Active, Exchange = exchange });
        }

        // TODO possible implementation of sell market order.
        /// <summary>
        /// Submits market order via Alpaca API.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="accountSettings"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task<bool> SubmitTrade(Order order, AccountSettings accountSettings, OrderQuantity quantity)
        {
            var client = GetAlpacaTradingClient(accountSettings.AlpacaApiKey, accountSettings.AlpacaSecretKey, order.UsePaperAccount);
            return await SubmitBuyOrder(order, quantity, client);            
        }

        /// <summary>
        /// Submits buy market order.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="quantity"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task<bool> SubmitBuyOrder(Order order, OrderQuantity quantity, IAlpacaTradingClient client)
        {
            var buyOrder = MarketOrder.Buy(order.ProductSymbol, quantity);

            IOrder? res = null;
            if (order.StopLoss > 0)
            {
                var stopLossPrice = (decimal)order.StopLoss;
                var stopLossOrder = buyOrder.StopLoss(stopLossPrice);

                if (order.TakeProfits > 0)
                {
                    var takeProfitPrice = (decimal)order.TakeProfits;
                    var takeProfitOrder = stopLossOrder.TakeProfit(takeProfitPrice);
                    res = await client.PostOrderAsync(takeProfitOrder);
                }
                else
                {
                    res = await client.PostOrderAsync(stopLossOrder);
                }
            }
            else if (order.TakeProfits > 0)
            {
                var takeProfitPrice = (decimal)order.TakeProfits;
                var takeProfitOrder = buyOrder.TakeProfit(takeProfitPrice);
                res = await client.PostOrderAsync(takeProfitOrder);
            }
            else
            {
                res = await client.PostOrderAsync(buyOrder);
            }

            return res != null;
        }

        /// <summary>
        /// Submits sell market order.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="quantity"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task<bool> SubmitSellOrder(Order order, OrderQuantity quantity, IAlpacaTradingClient client)
        {
            var sellOrder = MarketOrder.Sell(order.ProductSymbol, quantity);

            IOrder? res = null;
            if (order.StopLoss > 0)
            {
                var stopLossPrice = (decimal)order.StopLoss;
                var stopLossOrder = sellOrder.StopLoss(stopLossPrice);

                if (order.TakeProfits > 0)
                {
                    var takeProfitPrice = (decimal)order.TakeProfits;
                    var takeProfitOrder = stopLossOrder.TakeProfit(takeProfitPrice);
                    res = await client.PostOrderAsync(takeProfitOrder);
                }
                else
                {
                    res = await client.PostOrderAsync(stopLossOrder);
                }
            }
            else if (order.TakeProfits > 0)
            {
                var takeProfitPrice = (decimal)order.TakeProfits;
                var takeProfitOrder = sellOrder.TakeProfit(takeProfitPrice);
                res = await client.PostOrderAsync(takeProfitOrder);
            }
            else
            {
                res = await client.PostOrderAsync(sellOrder);
            }

            return res != null;
        }

        /// <summary>
        /// Cheks if provided account settings are valid and communication with alpaca trading client is possible.
        /// </summary>
        /// <param name="accountSettings"></param>
        /// <returns></returns>
        public async Task<bool> Validate(AccountSettings accountSettings)
        {
            var client = GetAlpacaTradingClient(accountSettings.AlpacaApiKey, accountSettings.AlpacaSecretKey, true);

            try
            {
                var me = await client.GetAccountAsync();
            }
            catch
            {
                return false;
            }
            
            return client != null;
        }

        /// <summary>
        /// Returns live or paper alpaca trading account.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="usePaperAccount"></param>
        /// <returns></returns>
        private IAlpacaTradingClient GetAlpacaTradingClient(string apiKey, string secretKey, bool usePaperAccount = false)
        {
            if (!usePaperAccount)
            {
                return Alpaca.Markets.Environments.Live.GetAlpacaTradingClient(new SecretKey(apiKey, secretKey));
            }

            return Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(apiKey, secretKey));
        }
    }
}
