using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingManBackend.DataLayer.Models
{
    public enum PositionType
    {
        Buy = 0,
        Sell = 1
    }

    /// <summary>
    /// Class representing proposed position of applications
    /// </summary>
    [Table("Positions")]
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PositionId { get; set; }
        public Guid UserId { get; set; }
        public string ProductSymbol { get; set; }
        public PositionType PositionType { get; set; }
        public double BaseValue { get; set; }
        public string NotificationName { get; set; }
    }
}
