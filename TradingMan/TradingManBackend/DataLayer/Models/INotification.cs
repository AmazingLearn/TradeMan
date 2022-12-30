using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingManBackend.DataLayer.Models
{
    /// <summary>
    /// Interface class for notifications contains common parameters.
    /// </summary>
    public abstract class INotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }
        // This is timeFrame in which the notification will be evaluated in
        public DateTime ExpiryDate { get; set; }
        public double BaseValue { get; set; }

        public bool Fullfilled { get; set; }
    }
}
