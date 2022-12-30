import {API_URL} from "../../util/config";
import {getUserId} from "../../util/Util";
import {
    INotificationDto,
    NewNotificationBasicDto,
    NewNotificationTrendDto,
    NotificationBasicDto,
    NotificationTrendDto
} from "./dtos/NotificationDtos";
import {ProductDto} from "./dtos/TradeDtos";

/**
 * Calls backend endpoint that will return all notifications for logged-in user.
 */
export async function getNotifications(): Promise<(NotificationBasicDto| NotificationTrendDto)[]> {

    console.log("Getting notifications");

    const res = await fetch(`${API_URL}/Notifications/GetNotifications/${getUserId()}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        }
    });

    if (res.status != 200) {
        alert(await res.text());
        return null;
    }

    return await res.json();
}

/**
 * Calls backend endpoint to create a new notification.
 * @param newNotificationDto
 */
export async function createNotification(newNotificationDto: NewNotificationBasicDto | NewNotificationTrendDto): Promise<boolean> {

    console.log("Creating notification");

    var res = null;

    if ("Direction" in newNotificationDto)
    {
        res = await fetch(`${API_URL}/Notifications/CreateNotificationBasic`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(newNotificationDto),
        });
    }
    else if ("Boundary" in newNotificationDto)
    {
        res = await fetch(`${API_URL}/Notifications/CreateNotificationTrend`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(newNotificationDto),
        });
    }
    else
    {
        alert("Unable to create notification, unable to determine notification type.");
        return;
    }

    if (res.status != 200) {
        alert(await res.text());
        return false;
    }

    return res.ok;
}

/**
 * Calls an endpoint that removes existing notification.
 * @param notificationDto
 */
export async function removeNotification(notificationDto: NewNotificationBasicDto | NewNotificationTrendDto): Promise<boolean> {

    var res = null;

    if ("direction" in notificationDto)
    {
        res = await fetch(`${API_URL}/Notifications/RemoveNotificationBasic`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(notificationDto),
        });
    }
    else if ("boundary" in notificationDto)
    {
        res = await fetch(`${API_URL}/Notifications/RemoveNotificationTrend`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(notificationDto),
        });
    }

    if (res.status != 200) {
        alert(await res.text());
        return false;
    }

    return res.ok;
}

/**
 * Calls an endpoint that returns notification based on notificaiton id.
 * @param notificationId
 */
export async function getSingleNotification(notificationId: number): Promise<INotificationDto> {

    console.log("Getting notification with Id: " + notificationId);

    const res = await fetch(`${API_URL}/Notifications/GetSingleNotification/${getUserId()}/${notificationId}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        }
    });

    if (res.status != 200) {
        alert(await res.text());
        return null;
    }

    return await res.json();
}

/**
 * Calls endpoint that returns all available products for given exchange etc nasdaq, .
 * @param exchange
 */
export async function getAllProducts(exchange: string): Promise<ProductDto[]> {

    console.log("Getting all products for exhange: " + exchange);

    const res = await fetch(`${API_URL}/Data/GetAllProducts/${getUserId()}/${exchange}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        }
    });

    if (res.status != 200) {
        alert(await res.text());
        return null;
    }

    return await res.json();
}