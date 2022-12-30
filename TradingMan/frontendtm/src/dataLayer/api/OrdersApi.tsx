import {API_URL} from "../../util/config";
import {OrderDto} from "./dtos/OrderDtos";

/**
 * Calls endpoint that will place an order with broker.
 * @param orderDto
 */
export async function createOrder(orderDto: OrderDto): Promise<boolean> {

    console.log("Placing order.");

    const res = await fetch(`${API_URL}/Orders/PlaceOrder`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(orderDto),
    });

    if (res.status != 200) {
        alert(await res.text());
        return false;
    }

    return res.ok;
}