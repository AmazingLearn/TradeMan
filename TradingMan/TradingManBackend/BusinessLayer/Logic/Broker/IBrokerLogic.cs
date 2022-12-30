using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.BusinessLayer.Logic
{
    /// <summary>
    /// Interface for broker operations
    /// </summary>
    public interface IBrokerLogic
    {
        /// <summary>
        /// Will return list of available products based on selected exhange
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="accountSettings"></param>
        /// <returns></returns>
        public Task<IEnumerable<Product>> GetAllAvailableProducts(string exchange, AccountSettings accountSettings);

        /// <summary>
        /// Submits a order to be processed by broker
        /// </summary>
        /// <param name="order"></param>
        /// <param name="accountSettings"></param>
        /// <returns></returns>
        public Task<bool> SubmitTrade(Order order, AccountSettings accountSettings);

        public Task<bool> Validate(AccountSettings accountSettings);
    }
}
