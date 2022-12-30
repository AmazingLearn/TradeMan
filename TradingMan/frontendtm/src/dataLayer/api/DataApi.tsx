import {API_URL} from "../../util/config";
import {StockDataDto} from "./dtos/StockDataDtos";

/**
 * Fetches available end of day data from backend for given symbol
 * @param symbol
 */
export async function getStockDailyData(symbol: string): Promise<StockDataDto[]> {

    console.log("Fetching data for symbol: " + symbol);

    const res = await fetch(`${API_URL}/Data/GetDailyData/${symbol}`, {
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