import {PositionDto} from "../api/dtos/PositionDtos";

/**
 * Contains classes related to proposed posions, mirros backend implementation.
 */
export enum PositionType {
    buy = 0,
    sell = 1
}

export const positionTypeOptions = [
    { value: PositionType.buy, label: "Buy" },
    { value: PositionType.sell, label: "Sell" }
]

export default class Position {
    readonly positionId: string
    readonly userId: string
    readonly productSymbol: string
    readonly positionType: PositionType
    readonly baseValue: number
    readonly currentPrice: number
    readonly notificationName: string

    constructor(props: Partial<Position>) {
        Object.assign(this, props);
    }

    static fromEmpty(): Position {
        return new Position({
            positionId: "",
            userId: "",
            productSymbol: "",
            positionType: PositionType.buy,
            baseValue: 0,
            currentPrice: 0,
            notificationName: "",
        });
    }

    static fromPositionDto(positionDto: PositionDto): Position {
        return new Position({
            positionId: positionDto.positionId,
            userId: positionDto.userId,
            productSymbol: positionDto.productSymbol,
            positionType: positionDto.positionType,
            baseValue: positionDto.baseValue,
            currentPrice: positionDto.currentPrice,
            notificationName: positionDto.notificationName,
        });
    }

    static toPositionDto(position: Position): PositionDto {
        return ({
            positionId: position.positionId,
            userId: position.userId,
            productSymbol: position.productSymbol,
            positionType: position.positionType,
            baseValue: position.baseValue,
            currentPrice: position.currentPrice,
            notificationName: position.notificationName,
        });
    }
}