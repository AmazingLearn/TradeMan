using TradingManBackend.DataLayer.ExternalAPI;
using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.BusinessLayer.Logic
{
    /// <summary>
    /// Service responsible for resolving stock data related logic
    /// </summary>
    public class StockDataLogic
    {
        // Using AlphaVantage as base, could be reworked to be decided at runtime.
        AlphaVantageApi _alphaVantage = new AlphaVantageApi();

        public readonly ILogger<StockDataLogic> _logger;

        public StockDataLogic(ILogger<StockDataLogic> logger)
        {
            _logger = logger;
        }

        public async Task<List<StockData>> GetDailyData(string symbol)
        {
            _logger.LogInformation($"Getting daily data for symbol: {symbol}");
            return _alphaVantage.GetDailyData(symbol);
        }

        public async Task<double> GetCurrentPrice(string symbol)
        {
            _logger.LogInformation($"Getting current price for symbol: {symbol}");
            return _alphaVantage.GetCurrentPrice(symbol);
        }
    }
}
