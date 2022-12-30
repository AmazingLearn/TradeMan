namespace TradingManBackend.DataLayer.Models
{
    public enum OrderVolumeType
    {
        Shares = 0,
        Currency = 1,
    }

    /// <summary>
    /// Class representing a single order.
    /// </summary>
    public class Order
    {
        public Guid UserId { get; set; }
        public string ProductSymbol { get; set; }
        public OrderVolumeType OrderVolumeType { get; set; }
        public double OrderVolume { get; set; }
        public bool UseStopLoss { get; set; }
        public double StopLoss { get; set; }
        public bool UseTakeProfits { get; set; }
        public double TakeProfits { get; set; }
        public bool UsePaperAccount { get; set; }
    }
}


