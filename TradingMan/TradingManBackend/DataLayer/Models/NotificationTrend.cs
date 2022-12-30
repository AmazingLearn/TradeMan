using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Class representing notification following trend parameters. Boundary specifies if support or resistance are to be evaluated.
/// </summary>
namespace TradingManBackend.DataLayer.Models
{
    public enum Boundary
    {
        Support = 0,
        Resistance = 1,
    }

    [Table("NotificationsTrend")]
    public class NotificationTrend : INotification
    {
        public Boundary Boundary { get; set; }
    }
}
