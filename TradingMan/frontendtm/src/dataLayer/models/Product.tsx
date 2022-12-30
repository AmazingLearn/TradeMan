import {ProductDto} from "../api/dtos/TradeDtos";

/**
 * Class representing a market product, mirrors backend implementation.
 */

export default class Product {
    readonly AssetId: string
    readonly Name: string
    readonly Symbol: string

    constructor(props: Partial<Product>) {
        Object.assign(this, props);
    }

    static FromProductDto(productDto: ProductDto) {
        return new Product({
            Name: productDto.name,
            Symbol: productDto.symbol
        });
    }
}