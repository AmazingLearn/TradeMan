using Microsoft.EntityFrameworkCore;
using TradingManBackend.DataLayer.Models;
using TradingManBackend.Models.Messaging;

namespace TradingManBackend.DataLayer
{
    /// <summary>
    /// Database context fot the backend application.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TelegramChannel> TelegramChannels { get; set; }
        public DbSet<AccountSettings> AccountSettings { get; set; }
        public DbSet<INotification> Notifications { get; set; }
        public DbSet<NotificationBasic> NotificationsBasic { get; set; }
        public DbSet<NotificationTrend> NotificationsTrend { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
