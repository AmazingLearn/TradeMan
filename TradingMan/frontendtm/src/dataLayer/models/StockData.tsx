import {StockDataDto} from "../api/dtos/StockDataDtos";

/**
 * Class representing stock data information in time, mirrors backend implementation.
 */

export default class StockData {
    readonly timeStamp: Date
    readonly open: number
    readonly high: number
    readonly low: number
    readonly close: number
    readonly volume: number

    constructor(props: Partial<StockData>) {
        Object.assign(this, props);
    }

    static fromStockDataDto(stockDataDto: StockDataDto): StockData {
        return new StockData({
            timeStamp: new Date(stockDataDto.timeStamp),
            open: stockDataDto.open,
            high: stockDataDto.high,
            low: stockDataDto.low,
            close: stockDataDto.close,
            volume: stockDataDto.volume
        });
    }
}