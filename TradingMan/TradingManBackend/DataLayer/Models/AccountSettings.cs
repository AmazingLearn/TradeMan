using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingManBackend.DataLayer.Models
{
    /// <summary>
    /// Class representing user account settings
    /// </summary>
    [Table("AccountSettings")]
    public class AccountSettings
    {
        [Key]
        public Guid UserId { get; set; }

        public string AlpacaApiKey { get; set; }

        public string AlpacaSecretKey { get; set; }

        public string TelegramUsername { get; set; }

        public bool UseTelegram { get; set; }
    }
}
