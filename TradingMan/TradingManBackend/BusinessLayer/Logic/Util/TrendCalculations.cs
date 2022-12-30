using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.BusinessLayer.Logic.Util
{
    /// <summary>
    /// Helper class that calculates support and resitance levels for given stock.
    /// Current implementation only calculates support and resistance one day back.
    /// </summary>
    public class TrendCalculations
    {
        private List<StockData> _stockData;

        /// <summary>
        /// </summary>
        /// <param name="stockData">EOD stock data</param>
        public TrendCalculations(List<StockData> stockData)
        {
            _stockData = stockData;
        }

        // TODO - Below is how to calculate further levels of support and resistance.
        // var res2 = (GetPivot(_stockData[1]) - sup1) + res1;
        // var sup2 = GetPivot(_stockData[1]) - (res2 - sup1);

        /// <summary>
        /// Returns support value based on previous day EOD data.
        /// </summary>
        /// <returns></returns>
        public double GetSupport()
        {
            var pivot = GetPivot(_stockData[0]);
            var suport = 2 * pivot - _stockData[0].Low;

            return suport;
        }

        /// <summary>
        /// Returns resistance value based on prevous day EOD data.
        /// </summary>
        /// <returns></returns>
        public double GetResistance()
        {           
            var pivot = GetPivot(_stockData[0]);
            var resistance = 2 * pivot - _stockData[0].Low;

            return resistance;
        }

        /// <summary>
        /// Calculates pivot point.
        /// </summary>
        /// <param name="stockData">EOD data for previous day.</param>
        /// <returns></returns>
        private static double GetPivot(StockData stockData)
        {
            return (stockData.High + stockData.Low + stockData.Close) /3.0;
        }
    }
}
