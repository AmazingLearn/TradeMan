using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Class repersenting basic type of notification. Direction specifies if notification is to be evaluated on increase or decrease in values.
/// </summary>
namespace TradingManBackend.DataLayer.Models
{
    public enum NotificationBasicType
    {
        AbsoluteChange = 0,
        PercentualChange = 1
    }

    public enum Direction
    {
        Increase = 0,
        Decrease = 1,
    }

    [Table("NotificationsBasic")]
    public class NotificationBasic : INotification
    {
        public NotificationBasicType NotificationBasicType { get; set; }

        public Direction Direction { get; set; }

        public double ExpectedChange { get; set; }
    }
}
