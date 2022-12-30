using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingManBackend.DataLayer.Models
{
    /// <summary>
    /// Class representing user of this application.
    /// </summary>
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
