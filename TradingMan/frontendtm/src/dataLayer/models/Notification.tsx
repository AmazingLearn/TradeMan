/**
 * Contains classes related to notifications, basic implementation mirross backend.
 * Minor disparities with beckend are due to needs of frontend application.
 * Also containes helper containers with options for select elements.
 */
export enum NotificationBasicType {
    absoluteChange = 0,
    percentualChange = 1,
}

export enum Direction {
    increase = 0,
    decrease =1
}

export enum Boundary {
    support = 0,
    resistance = 1
}

export enum NotificationType {
    unknown,
    absoluteChange,
    percentualChange,
    trendBoundaries
}

export const typeOptions = [
    { value: NotificationType.unknown, label: "" },
    { value: NotificationType.absoluteChange, label: 'Absolute change' },
    { value: NotificationType.percentualChange, label: 'Percentual change' },
    { value: NotificationType.trendBoundaries, label: 'Trend boundaries' }
]

export const directionOptions = [
    { value: Direction.increase, label: 'Increase' },
    { value: Direction.decrease, label: 'Decrease' }
]

export const boundaryOptions = [
    { value: Boundary.support, label: 'Support' },
    { value: Boundary.resistance, label: 'Resistance' }
]

export default class INotification
{
    readonly notificationId: number
    readonly userId: string
    readonly name: string
    readonly symbol: string
    readonly expiryDate: string
    readonly baseValue: number
    readonly fullfilled: boolean

    constructor(props: Partial<INotification>) {
        Object.assign(this, props);
    }

    static fromEmpty(): INotification {
        console.log("Big oof Shouldnt be called.");
        return new INotification({
            notificationId: 0,
            userId: "",
            name: "",
            symbol: "",
            expiryDate: "",
            baseValue: 0,
            fullfilled: false
        });
    }
}

export class NotificationBasic extends INotification
{
    readonly expectedChange: number
    readonly notificationType: NotificationBasicType
    readonly direction: Direction

    constructor(props: Partial<NotificationBasic>) {
        super(props);
        Object.assign(this, props);
    }

    static fromEmpty(): INotification {
        return new NotificationBasic({
            notificationId: 0,
            userId: "",
            name: "",
            symbol: "",
            notificationType: NotificationBasicType.absoluteChange,
            expectedChange: 0,
            expiryDate: "",
            baseValue: 0,
            direction: Direction.increase,
            fullfilled: false
        });
    }
}

export class NotificationTrend extends INotification
{
    readonly boundary: Boundary

    constructor(props: Partial<NotificationTrend>) {
        super(props);
        Object.assign(this, props);
    }

    static fromEmpty(): INotification {
        return new NotificationTrend({
            notificationId: 0,
            userId: "",
            name: "",
            symbol: "",
            expiryDate: "",
            baseValue: 0,
            boundary: Boundary.support,
            fullfilled: false
        });
    }
}