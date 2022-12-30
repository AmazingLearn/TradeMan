using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingManBackend.Models.Messaging
{
    /// <summary>
    /// Class representing users required telegram channel info
    /// </summary>
    [Table("TelegramChannels")]
    public class TelegramChannel
    {
        [Key]
        public Guid UserId { get; set; }
        public long ChannelId { get; set; }  
    }
}
