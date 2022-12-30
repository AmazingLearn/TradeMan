import {Boundary, Direction, NotificationBasicType} from "../../models/Notification";

export interface INewNotificationDto {
    UserId: string,
    Name: string,
    Symbol: string,
    ExpiryDate: string,
    BaseValue: number,
}

export interface NewNotificationBasicDto extends INewNotificationDto {
    NotificationType: NotificationBasicType,
    ExpectedChange: number,
    Direction: Direction
}

export interface NewNotificationTrendDto extends INewNotificationDto {
    Boundary: Boundary
}

export interface INotificationDto {
    NotificationId: number,
    UserId: string,
    Name: string,
    Symbol: string,
    ExpiryDate: string,
    BaseValue: number,
    Fullfilled: boolean
}

export interface NotificationBasicDto extends  INotificationDto {
    NotificationType: NotificationBasicType,
    ExpectedChange: number,
    Direction: Direction,

}

export interface NotificationTrendDto extends INotificationDto {
    Boundary: Boundary
}