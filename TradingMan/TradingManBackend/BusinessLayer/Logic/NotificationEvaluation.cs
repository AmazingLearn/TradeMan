using TradingManBackend.BusinessLayer.Logic.Messaging;
using TradingManBackend.BusinessLayer.Logic.Util;
using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;
using TradingManBackend.Util;

namespace TradingManBackend.BusinessLayer.Logic
{
    /// <summary>
    /// Class responsible for evaluation of notifications and creation of proposed positions
    /// from cron job
    /// </summary>
    public class NotificationEvaluation
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<NotificationEvaluation> _logger;
        private readonly StockDataLogic _dataLogic;
        private readonly MessagingLogic _messagingLogic;

        public NotificationEvaluation(
            DatabaseContext context,
            ILogger<NotificationEvaluation> logger,
            StockDataLogic dataLogic,
            MessagingLogic messagingLogic)
        {
            _context = context;
            _logger = logger;
            _dataLogic = dataLogic;
            _messagingLogic = messagingLogic;
        }

        /// <summary>
        /// Entry point to initiate notification evaluations
        /// </summary>
        public async void EvaluateNotifications()
        {
            _logger.LogInformation("Starting notification evaluation.");
            var notifications = GetUnfulfilledActiveNotificationsFromDb();
            var usersSent = new List<Guid>();
            foreach (var notification in notifications)
            {
                usersSent = await EvaluateNotification(notification, usersSent);
            }
        }

        /// <summary>
        /// Goes thorugh all notifications and selects unfulfilled and non-expired notifications.
        /// </summary>
        /// <returns></returns>
        private List<INotification> GetUnfulfilledActiveNotificationsFromDb()
        {
            var unfullfilledNotifications = new List<INotification>();

            if (!_context.NotificationsBasic.Any() && !_context.NotificationsTrend.Any())
            {
                return unfullfilledNotifications;
            }

            var notificationsBasic = _context.NotificationsBasic.Where(n => n.ExpiryDate > DateTime.Now && !n.Fullfilled).ToList();
            unfullfilledNotifications.AddRange(notificationsBasic);

            var notificationsTrend = _context.NotificationsTrend.Where(n => n.ExpiryDate > DateTime.Now && !n.Fullfilled).ToList();
            unfullfilledNotifications.AddRange(notificationsTrend);

            return unfullfilledNotifications;
        }

        /// <summary>
        /// Evaluates fulfillment condition of notification, sets appropriate flag and sends a message to the user if conditions are met.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="usersSent"></param>
        /// <returns></returns>
        private async Task<List<Guid>> EvaluateNotification(INotification notification, List<Guid> usersSent)
        {
            _logger.LogInformation($"Evaluating notification: {notification}");
            if (await CheckPrice(notification))
            {
                // update contidition to fullfilled so that the users won't get spammed.
                notification.Fullfilled = true;
                _context.SaveChanges();

                var positionId = GeneratePosition(notification);

                // Send a message to the user
                if (!usersSent.Contains(notification.UserId))
                {
                    usersSent.Add(notification.UserId);
                    SendMessage(positionId, notification.UserId, notification.Symbol, notification.Name);
                }
            }

            return usersSent;
        }

        /// <summary>
        /// Cheks price curent price and base price compared to notifications settings.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        private async Task<bool> CheckPrice(INotification notification)
        {
            var conditionFulfilled = false;
            double currentPrice = 0;
            try
            {
                currentPrice = await _dataLogic.GetCurrentPrice(notification.Symbol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            if (notification.GetType() == typeof(NotificationBasic))
            {
                NotificationBasic notificationBasic = (NotificationBasic)notification;

                switch (notificationBasic.NotificationBasicType)
                {
                    case NotificationBasicType.AbsoluteChange:
                        conditionFulfilled = CheckAbsoluteChange(
                            notificationBasic.BaseValue,
                            notificationBasic.ExpectedChange,
                            currentPrice,
                            notificationBasic.Direction);
                        break;
                    case NotificationBasicType.PercentualChange:
                        conditionFulfilled = CheckPercentualChange(
                            notificationBasic.BaseValue,
                            notificationBasic.ExpectedChange,
                            currentPrice,
                            notificationBasic.Direction);
                        break;
                    default:
                        conditionFulfilled = false;
                        break;
                }
            }

            if (notification.GetType() == typeof(NotificationTrend))
            {
                NotificationTrend notificationTrend = (NotificationTrend)notification;

                List<StockData> dailyData = new List<StockData>();
                try
                {
                    dailyData = await _dataLogic.GetDailyData(notification.Symbol);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return false;
                }

                conditionFulfilled = CheckTrendBoundaries(currentPrice, notificationTrend.Boundary, dailyData);
            }

            return conditionFulfilled;
        }

        private bool CheckAbsoluteChange(double basePrice, double expectedChange, double currentPrice, Direction direction)
        {
            switch (direction)
            {
                case Direction.Increase:
                    return basePrice + expectedChange <= currentPrice;
                case Direction.Decrease:
                    return basePrice - expectedChange >= currentPrice;
            }

            return false;
        }

        private bool CheckPercentualChange(double basePrice, double expectedChange, double currentPrice, Direction direction)
        {
            var percentualChange = ((currentPrice - basePrice) / basePrice) * 100;
            switch (direction)
            {
                case Direction.Increase:
                    return expectedChange <= percentualChange;
                case Direction.Decrease:
                    return -expectedChange >= percentualChange;
            }

            return false;
        }

        private bool CheckTrendBoundaries(double currentPrice, Boundary boundary, List<StockData> stockData)
        {
            var calc = new TrendCalculations(stockData);

            switch(boundary)
            {
                case Boundary.Support:
                    return calc.GetSupport() >= currentPrice;
                case Boundary.Resistance:
                    return calc.GetResistance() <= currentPrice;
            }
            
            return false;
        }

        /// <summary>
        /// Generates proposed position for given notification.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        private Guid GeneratePosition(INotification notification)
        {
            _logger.LogInformation($"Generating position for notification: {notification}");

            // TODO other positions than buy are currently not implemented
            var positionType = PositionType.Buy;
            
            if (notification.GetType() == typeof(NotificationBasic))
            {
                NotificationBasic notificationBasic = (NotificationBasic)notification;
                if (notificationBasic.Direction == Direction.Increase)
                {
                    positionType = PositionType.Sell;
                }
            }

            if (notification.GetType() == typeof(NotificationTrend))
            {
                NotificationTrend notificationTrend = (NotificationTrend)notification;
                if (notificationTrend.Boundary == Boundary.Resistance)
                {
                    positionType = PositionType.Sell;
                }
            }

            var position = new Position
            {
                UserId = notification.UserId,
                ProductSymbol = notification.Symbol,
                
                // Thisis just temprorary to make sure, need to implement the sell functionality still
                PositionType = positionType,
                BaseValue = notification.BaseValue,
                NotificationName = notification.Name,
            };

            _context.Add(position);
            _context.SaveChanges();

            return position.PositionId;
        }

        /// <summary>
        /// Sends message about notification fullfillment to all registered channels.
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="userId"></param>
        /// <param name="symbol"></param>
        /// <param name="notificationName"></param>
        private void SendMessage(Guid positionId, Guid userId, string symbol, string notificationName)
        {
            _logger.LogInformation($"Sending new position notification to userId: {userId} with positionId: {positionId}");

            var message = new Message
            {
                Subject = $"Notification: {notificationName} for product {symbol} has met set criteria",
                Body = $"Check proposed positions at: {Constants.FrontendUrl}"
            };

            _messagingLogic.SendMessageToAllRegisteredChannels(message, userId);
        }
    }
}
