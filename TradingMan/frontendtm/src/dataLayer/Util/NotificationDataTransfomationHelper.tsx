import {NotificationBasicDto, NotificationTrendDto} from "../api/dtos/NotificationDtos";
import {
    NotificationBasic,
    NotificationBasicType,
    NotificationTrend,
    NotificationType
} from "../models/Notification";


/**
 * Class with static methods to help convert data types related to notifications.
 */
export default class NotificationDataTransfomationHelper
{
    /**
     * Converts notificationDtos to instances of notification class.
     * @param notificationDto
     */
    static fromNotificationDto(notificationDto: NotificationTrendDto | NotificationBasicDto): NotificationBasic | NotificationTrend {

        if ("Boundary" in notificationDto)
        {
            return new NotificationTrend({
                notificationId: notificationDto.NotificationId,
                userId: notificationDto.UserId,
                name: notificationDto.Name,
                symbol: notificationDto.Symbol,
                expiryDate: notificationDto.ExpiryDate,
                baseValue: notificationDto.BaseValue,
                boundary: notificationDto.Boundary,
                fullfilled: notificationDto.Fullfilled
            });
        }
        else if ("Direction" in notificationDto)
        {
            return new NotificationBasic({
                notificationId: notificationDto.NotificationId,
                userId: notificationDto.UserId,
                name: notificationDto.Name,
                symbol: notificationDto.Symbol,
                notificationType: notificationDto.NotificationType,
                expectedChange: notificationDto.ExpectedChange,
                expiryDate: notificationDto.ExpiryDate,
                baseValue: notificationDto.BaseValue,
                direction: notificationDto.Direction,
                fullfilled: notificationDto.Fullfilled
            });
        }

        throw new Error("Unexpected notificationDto type.");
    }

    /**
     * Converts instances of notification class to notificationDtos.
     * @param notificationDto
     */
    static toNotificationDto(notification: NotificationTrend | NotificationBasic): NotificationTrendDto | NotificationBasicDto {

        if ("boundary" in notification)
        {
            return ( {
                NotificationId: notification.notificationId,
                UserId: notification.userId,
                Name: notification.name,
                Symbol: notification.symbol,
                ExpiryDate: notification.expiryDate,
                BaseValue: notification.baseValue,
                Boundary: notification.boundary,
                Fullfilled: notification.fullfilled
            });
        }
        else if ("direction" in notification)
        {
            return ( {
                NotificationId: notification.notificationId,
                UserId: notification.userId,
                Name: notification.name,
                Symbol: notification.symbol,
                NotificationType: notification.notificationType,
                ExpectedChange: notification.expectedChange,
                ExpiryDate: notification.expiryDate,
                BaseValue: notification.baseValue,
                Direction: notification.direction,
                Fullfilled: notification.fullfilled
            });
        }

        throw new Error("Unexpected notificationDto type.");
    }

    /**
     * Helper method thar returns appropriate notificationBasicType based on notification type selection.
     * @param notificationType
     */
    static getNotificationBasicType(notificationType: NotificationType): NotificationBasicType
    {
        switch (notificationType)
        {
            case NotificationType.absoluteChange:
                return NotificationBasicType.absoluteChange;
            case NotificationType.percentualChange:
                return NotificationBasicType.percentualChange;
            default:
                throw new Error("Unexpected notificationType in notificationType to notificationTypeBasic conversion.");
        }
    }
}