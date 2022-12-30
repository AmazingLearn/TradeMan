import {API_URL} from "../../util/config";
import {getUserId} from "../../util/Util";
import { PositionDto } from "./dtos/PositionDtos";

/**
 * Calls endpoint to return a specific proposed position with given Id.
 * @param positionId
 */
export async function getSinglePosition(positionId: string): Promise<PositionDto> {

    console.log("Getting notification with Id: " + positionId);

    const res = await fetch(`${API_URL}/Positions/GetPosition/${getUserId()}/${positionId}`, {
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
 * Returns all proposed positions for logged-in user.
 */
export async function getAllPositions(): Promise<PositionDto[]> {

    console.log("Getting notification for user Id: " + getUserId());

    const res = await fetch(`${API_URL}/Positions/GetAllPositions/${getUserId()}`, {
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
 * Rejects and deletes proposed possition for logged-in user.
 * @param positionId
 */
export async function rejectPosition(positionId: string): Promise<boolean> {

    console.log("Removing position with positionId: " + positionId);

    const res = await fetch(`${API_URL}/Positions/RemovePisition/${positionId}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (res.status != 200) {
        alert(await res.text());
        return false;
    }

    return true;
}