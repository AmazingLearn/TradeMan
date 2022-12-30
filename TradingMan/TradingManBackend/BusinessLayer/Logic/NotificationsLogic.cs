using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.BusinessLayer.Logic
{
    /// <summary>
    /// Service containing logic for operating with notifications
    /// </summary>
    public class NotificationsLogic
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<NotificationsLogic> _logger;

        public NotificationsLogic(DatabaseContext context, ILogger<NotificationsLogic> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        /// <summary>
        /// Retrieves all notifications from db for given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<INotification> GetNotifications(Guid userId)
        {
            _logger.LogInformation($"Getting notifications for userId: [{userId}]");

            IEnumerable<INotification> notificationsBasic = _context.NotificationsBasic.Where(notification => notification.UserId == userId);
            IEnumerable<INotification> notificationsTrend = _context.NotificationsTrend.Where(notification => notification.UserId == userId);
            var notificationsAll = notificationsBasic.Concat(notificationsTrend);

            return notificationsAll.OrderBy(x => x.NotificationId);
        }

        /// <summary>
        /// Creates a new entry in DB for given notification
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public void CreateNotification(INotification notification)
        {
            _logger.LogInformation($"Creating notification for userId: [{notification.UserId}]");
            _context.Add(notification);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes notification entry from DB
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public void RemoveNotification(INotification notification)
        {
            _logger.LogInformation($"Removing notification with notificationId: [{notification.NotificationId}]");
            _context.Remove(notification);
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets notification details for given userId and notificationID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public INotification GetSingleNotification(Guid userId, int notificationId)
        {
            _logger.LogInformation($"Getting notification for userId: [{userId}] and notificationId: [{notificationId}].");
            
            if (_context.NotificationsBasic.Any(notification => notification.UserId == userId && notification.NotificationId == notificationId))
            {
                return _context.NotificationsBasic.FirstOrDefault(notification => notification.UserId == userId && notification.NotificationId == notificationId);
            }

            return _context.NotificationsTrend.FirstOrDefault(notification => notification.UserId == userId && notification.NotificationId == notificationId);
        }
    }
}
