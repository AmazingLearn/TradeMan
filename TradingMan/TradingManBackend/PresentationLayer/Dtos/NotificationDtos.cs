using TradingManBackend.DataLayer.Models;

/// <summary>
/// Contains classes for transfering notifications related objects between frontend and backend
/// </summary>
namespace TradingManBackend.PresentationLayer.Dtos
{
    public abstract class INewNotificationDto
    {
        public int NotificationId { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }
        public string ExpiryDate { get; set; }
        public double BaseValue { get; set; }
    }

    public class NewNotificationBasicDto : INewNotificationDto
    {
        public NotificationBasicType NotificationType { get; set; }
        public Direction Direction { get; set; }

        public double ExpectedChange { get; set; }
    }

    public class NewNotificationTrendDto : INewNotificationDto
    {
        public Boundary Boundary { get; set; }
    }

    public abstract class INotificationDto
    {
        public int NotificationId { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }
        public string ExpiryDate { get; set; }
        public double BaseValue { get; set; }
        public bool Fullfilled { get; set; }
    }
    
    public class NotificationBasicDto : INotificationDto
    {
        public NotificationBasicType NotificationType { get; set; }
        public Direction Direction { get; set; }

        public double ExpectedChange { get; set; }
    }

    public class NotificationTrendDto : INotificationDto
    {
        public Boundary Boundary { get; set; }
    }

    // TODO
    // Some conversions are ignoring timezone, when we get datetime in string from frontend,
    // this string contains information about timezone, currently this information is trimmed and ignored.

    /// <summary>
    /// Contains methods to help transform between user related data transfer objects and models.
    /// </summary>
    public class NotificationDtoHelper
    {
        public static INotification FromNewNotificationDto(NewNotificationBasicDto newNotificationDto)
        {
            return new NotificationBasic
            {
                UserId = newNotificationDto.UserId,
                Name = newNotificationDto.Name,
                Symbol = newNotificationDto.Symbol,
                NotificationBasicType = newNotificationDto.NotificationType,
                ExpectedChange = newNotificationDto.ExpectedChange,
                
                // Ignoring timezone
                ExpiryDate = DateTime.Parse(newNotificationDto.ExpiryDate.Substring(0, 15)),
                BaseValue = newNotificationDto.BaseValue,
                Direction = newNotificationDto.Direction,
            };
        }

        public static INotification FromNewNotificationDto(NewNotificationTrendDto newNotificationDto)
        {
            return new NotificationTrend
            {
                UserId = newNotificationDto.UserId,
                Name = newNotificationDto.Name,
                Symbol = newNotificationDto.Symbol,
                
                // Ignoring timezone
                ExpiryDate = DateTime.Parse(newNotificationDto.ExpiryDate.Substring(0, 15)),
                BaseValue = newNotificationDto.BaseValue,
                Boundary = newNotificationDto.Boundary,
            };
        }

        public static INotification FromNotificationDto(NotificationBasicDto notification)
        {
            return new NotificationBasic
            {
                UserId = notification.UserId,
                Name = notification.Name,
                Symbol = notification.Symbol,
                NotificationBasicType = notification.NotificationType,
                ExpectedChange = notification.ExpectedChange,

                // Ignoring timezone
                ExpiryDate = DateTime.Parse(notification.ExpiryDate),
                BaseValue = notification.BaseValue,
                Direction = notification.Direction,
                Fullfilled = notification.Fullfilled
            };
        }

        public static INotification FromNotificationDto(NotificationTrendDto notification)
        {
            return new NotificationTrend
            {
                UserId = notification.UserId,
                Name = notification.Name,
                Symbol = notification.Symbol,

                // Ignoring timezone
                ExpiryDate = DateTime.Parse(notification.ExpiryDate),
                BaseValue = notification.BaseValue,
                Boundary = notification.Boundary,
                Fullfilled = notification.Fullfilled,
            };
        }

        public static INotificationDto ToNotificationDto(INotification notification)
        { 
            if (notification.GetType() == typeof(NotificationBasic))
            {
                NotificationBasic basic = (NotificationBasic)notification;

                return new NotificationBasicDto
                {
                    UserId = basic.UserId,
                    Name = basic.Name,
                    Symbol = basic.Symbol,
                    NotificationType = basic.NotificationBasicType,
                    ExpectedChange = basic.ExpectedChange,

                    ExpiryDate = basic.ExpiryDate.ToString(),
                    BaseValue = basic.BaseValue,
                    Direction = basic.Direction,
                    Fullfilled = basic.Fullfilled,
                };
            }

            if (notification.GetType() == typeof(NotificationTrend))
            {
                NotificationTrend trend = (NotificationTrend)notification;

                return new NotificationTrendDto
                {
                    UserId = trend.UserId,
                    Name = trend.Name,
                    Symbol = trend.Symbol,

                    // Ignoring timezone
                    ExpiryDate = trend.ExpiryDate.ToString(),
                    BaseValue = trend.BaseValue,
                    Boundary = trend.Boundary,
                    Fullfilled = trend.Fullfilled,
                };
            }

            throw new Exception("Unexpected notification type when converting notification to notificationDTO.");
        }   
    }
}
