namespace TradingManBackend.DataLayer.Models
{
    /// <summary>
    /// Class representing point in time series data.
    /// </summary>
    public class StockData
    {
        public DateTime TimeStamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
    }
}
