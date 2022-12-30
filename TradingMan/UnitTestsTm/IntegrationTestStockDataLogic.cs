using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradingManBackend.BusinessLayer.Logic;

namespace UnitTestsTm
{
    [TestClass]
    public class IntegrationTestStockDataLogic
    {
        // Logic services
        private static StockDataLogic _sLogic;

        // Test data
        private const string _testProductSymbol = "AAPL";
        private const string _InvalidtestProductSymbol = "INVALID_SYMBOL";

        // Sets up the class - is ran only once before tests
        [ClassInitialize]
        public static void Init(TestContext c)
        {
            // Prepare loggers
            var sMockLogger = new Mock<ILogger<StockDataLogic>>();

            // Initialize services
            _sLogic = new StockDataLogic(sMockLogger.Object);
        }

        [TestMethod]
        public async Task TestDailyData()
        {
            // Test daily data retrieval
            var dailyData = await _sLogic.GetDailyData(_testProductSymbol);
            Assert.IsNotNull(dailyData);
            Assert.IsTrue(dailyData.Any());
        }

        [TestMethod]
        public async Task TestDailyDataInvalidSymbol()
        {
            // This will only throw when trying to convert Task into StockData
            var exceptionOccured = false;
            try
            {
                var dailyData = await _sLogic.GetDailyData("_InvalidtestProductSymbol");
            }
            catch
            {
                exceptionOccured = true;
            }

            Assert.IsTrue(exceptionOccured);
        }

        [TestMethod]
        public async Task TestCurrentPrice()
        {
            double currentPrice = double.MinValue;
            currentPrice = await _sLogic.GetCurrentPrice(_testProductSymbol);
            Assert.IsNotNull(currentPrice);
            Assert.AreNotEqual(double.MinValue, currentPrice, 1);
        }

        [TestMethod]
        public async Task TestCurrentPriceInvalidSymbol()
        {
            // This will only throw when trying to convert Task into double
            var exceptionOccured = false;
            try
            {
                var currPrice = await _sLogic.GetCurrentPrice(_InvalidtestProductSymbol);
            }
            catch
            {
                exceptionOccured = true;
            }

            Assert.IsTrue(exceptionOccured);
        }
    }
}
