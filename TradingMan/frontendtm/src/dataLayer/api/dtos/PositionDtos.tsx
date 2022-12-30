import {PositionType} from "../../models/Position";

export interface PositionDto {
    positionId: string,
    userId: string,
    productSymbol: string,
    positionType: PositionType,
    baseValue: number,
    currentPrice: number,
    notificationName: string
}