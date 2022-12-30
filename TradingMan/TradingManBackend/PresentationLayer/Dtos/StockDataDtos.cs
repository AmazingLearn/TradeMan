using TradingManBackend.DataLayer.Models;

/// <summary>
/// Contains classes for transfering stock data related objects between frontend and backend
/// </summary>
namespace TradingManBackend.PresentationLayer.Dtos
{
    public class StockDataDto : StockData
    {

    }

    public class ProductDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Contains methods to help transform between market data related data transfer objects and models 
    /// </summary>
    public class StockDataDtoHelper
    {
        public static StockDataDto ToStockDataDto(StockData stockData)
        {
            return new StockDataDto
            {
                TimeStamp = stockData.TimeStamp,
                Open = stockData.Open,
                High = stockData.High,
                Low = stockData.Low,
                Close = stockData.Close,
                Volume = stockData.Volume,
            };
        }
    }

    /// <summary>
    /// Contains methods to help transform between product related data transfer objects and models 
    /// </summary>
    public class ProductDtoHelper
    {
        public static ProductDto ToProductDto(Product product)
        {
            return new ProductDto
            {
                Name = product.Name,
                Symbol = product.Symbol
            };
        }
    }
}
