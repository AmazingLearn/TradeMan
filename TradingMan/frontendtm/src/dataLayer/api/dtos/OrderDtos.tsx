export enum OrderVolumeType {
    shares = 0,
    currency = 1
}

export const orderVolumneTypeOptions = [
    { value: OrderVolumeType.shares, label: "Shares" },
    { value: OrderVolumeType.currency, label: "Currency" }
]

export interface OrderDto {
    userId: string
    productSymbol: string
    orderVolumeType: OrderVolumeType
    orderVolume: number
    useStopLoss: boolean
    stopLoss: number
    useTakeProfits: boolean
    takeProfits: number
    usePaperAccount: boolean
}