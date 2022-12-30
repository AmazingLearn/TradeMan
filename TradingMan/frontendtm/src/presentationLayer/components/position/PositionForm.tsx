import React, {useEffect, useState} from "react";
import {getSinglePosition, rejectPosition} from "../../../dataLayer/api/PositionsApi";
import Position, {PositionType, positionTypeOptions} from "../../../dataLayer/models/Position";
import LineChart from "../graph/LineChart";
import StockData from "../../../dataLayer/models/StockData";
import {StockDataDto} from "../../../dataLayer/api/dtos/StockDataDtos";
import {getStockDailyData} from "../../../dataLayer/api/DataApi";
import Select from "react-select";
import {OrderVolumeType, orderVolumneTypeOptions} from "../../../dataLayer/api/dtos/OrderDtos";
import {createOrder} from "../../../dataLayer/api/OrdersApi";
import {getUserId} from "../../../util/Util";
import {useNavigate} from "react-router-dom";
import {validateNumericalInput} from "../../util/Util";

export interface IPositionFromProps {}

/**
 * Element for displaying existing proposed possitions.
 * @param props
 * @constructor
 */
function PositionForm(props: IPositionFromProps) {

    const navigate = useNavigate();
    const [position, setPosition] = useState<Position>()
    const [display, setDisplay] = useState(false);
    const [graphData, setGraphData] = useState<StockDataDto[]>([]);

    // TODO
    // Currently not used.
    const [positionType, setPositionType] = useState<PositionType>(PositionType.buy);

    const [stopLoss, setStopLoss] = useState<number>(-1);
    const [takeProfits, setTakeProfits] = useState<number>(-1);

    const [volumeName, setVolumeName] = useState("shares");

    const [orderVolumeType, setOrderVolumeType] = useState<OrderVolumeType>(0);
    const [orderAmount, setOrderAmount] = useState<number>(0);
    const [usePaperAccount, setUsePaperAccount] = useState(true);

    const [useStopLoss, setUseStopLoss] = useState(false);
    const [useTakeProfit, setUseTakeProfit] = useState(false);

    useEffect(() => {
        const idFromPath = window.location.pathname.split("/")[2];
        fetchPositionData(idFromPath);
    },[]);

    const fetchPositionData = async (positionId: string) => {
        const pos = await getSinglePosition(positionId);
        setPosition(pos);
        await fetchGraphData(pos.productSymbol);
        setDisplay(true);
    }

    async function fetchGraphData(symbol: string):Promise<boolean> {
        console.log("Fetching data for" + symbol);
        const data = await getStockDailyData(symbol);
        await setGraphData(data);
        return true;
    }

    function graph() {
        return (
            <LineChart data={graphData.map(item => {return StockData.fromStockDataDto(item)})} symbol={position.productSymbol} />
        );
    }

    const handleStopLossChange = (stopLossChanged: string) => {
        if(validateNumericalInput(stopLossChanged))
        {
            setStopLoss(Number(stopLossChanged));
        }
        else
        {
            setStopLoss(Number(0));
        }
    }

    const handleTakeProfitsChange = (takeProfitsChanged: string) => {
        if(validateNumericalInput(takeProfitsChanged))
        {
            setTakeProfits(Number(takeProfitsChanged));
        }
        else
        {
            setTakeProfits(0);
        }
    }

    const handlePositionTypeChange = (option: number) => {
        setPositionType(positionTypeOptions[option].value);
    }

    const handleUsePaperAccount = () => {
        setUsePaperAccount(!usePaperAccount);
    }

    const handleUseStopLoss = () => {
        setUseStopLoss(!useStopLoss)
    }

    const handleUseTakePRofit = () => {
        setUseTakeProfit(!useTakeProfit)
    }

    /** This thing is not working
     */
    const handleOrderVolumeTypeChange = (option: OrderVolumeType) => {

        setOrderVolumeType(orderVolumneTypeOptions[option].value);

        console.log(option)

        if (option == OrderVolumeType.currency)
        {
            setVolumeName("USD");
        }
        else
        {
            setVolumeName("shares");
        }
    }

    const handleOrderAmountChange = (orderAmountChange: string) => {
        setOrderAmount(Number(orderAmountChange));
    }

    const submitOrder = async () => {
        const res = await createOrder({
           userId: getUserId(),
           productSymbol: position.productSymbol,
           orderVolumeType: orderVolumeType,
           orderVolume: orderAmount,
           useStopLoss: useStopLoss,
           stopLoss: stopLoss,
           useTakeProfits: useTakeProfit,
           takeProfits: takeProfits,
           usePaperAccount: usePaperAccount
        });

        if (res == true)
        {
            navigate("/positions");
        }
    }

    const deletePosition = async () => {
        const res = await rejectPosition(position.positionId);
        if (res)
        {
            navigate("/positions");
        }
    }

    function displayPositionDetails() {
        return (
            <div className="grid place-items-center fit-content h-screen">

                <form className="px-8 py-2 h-full w-3/4">
                    <h2 className="py-4 mx-8 text-left text-3xl font-medium text-gray-900">Position for: {position.notificationName}</h2>

                    <div className="h-full flex">
                        <div className="w-1/4">
                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Product
                            </label>
                            <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{position.productSymbol}</h2>

                            {
                                // TODO
                                // Sell or other types of orders are not implemented yet
                                /*
                                <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                    Position type
                                </label>
                                <Select
                                options={positionTypeOptions}
                                onChange={e => handlePositionTypeChange(e.value)}
                                defaultValue={positionTypeOptions[position.positionType]}
                                />
                                */
                            }

                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Base value
                            </label>
                            <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{position.baseValue + " USD"}</h2>

                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Current price
                            </label>
                            <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{position.currentPrice + " USD"}</h2>

                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Order volume type
                            </label>
                            <Select
                                options={orderVolumneTypeOptions}
                                onChange={e => handleOrderVolumeTypeChange(e.value)}
                                defaultValue={orderVolumneTypeOptions[0]}
                            />

                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Order amount - in {volumeName}
                            </label>
                            <input
                                className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full"
                                type="text"
                                onKeyPress={(event) => {
                                    if (!/[0-9.]/.test(event.key)) {
                                        event.preventDefault();
                                    }
                                }}
                                onChange={e => handleOrderAmountChange(e.target.value)}/>

                            <div className="py-2 sm:flex sm:flex-row space-x-4">
                                <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                    Use paper account
                                </label>
                                <input type="checkbox" defaultChecked={usePaperAccount}  onChange={handleUsePaperAccount}></input>
                            </div>

                            <div className="py-2 sm:flex sm:flex-row space-x-4">
                                <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                    Use stop loss
                                </label>
                                <input type="checkbox" defaultChecked={useStopLoss}  onChange={handleUseStopLoss}></input>
                            </div>

                            <div className="py-2 sm:flex sm:flex-row space-x-4">
                                <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                    Use take profit
                                </label>
                                <input type="checkbox" defaultChecked={useTakeProfit}  onChange={handleUseTakePRofit}></input>
                            </div>

                            {useStopLoss ?
                                <div>
                                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                        Stop loss - USD value per product
                                    </label>
                                    <input
                                        className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full"
                                        type="text"
                                        onKeyPress={(event) => {
                                            if (!/[0-9.]/.test(event.key)) {
                                                event.preventDefault();
                                            }
                                        }}
                                        onChange={e => handleStopLossChange(e.target.value)}
                                    />
                                </div>
                                :
                                ""
                            }

                            {useTakeProfit ?
                                <div>
                                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                        Take profit - USD value per product
                                    </label>
                                    <input
                                        className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full"
                                        type="text"
                                        onKeyPress={(event) => {
                                            if (!/[0-9]/.test(event.key)) {
                                                event.preventDefault();
                                            }
                                        }}
                                        onChange={e => handleTakeProfitsChange(e.target.value)}/>
                                </div>
                                :
                                ""
                            }

                            <div className="py-8 sm:flex sm:flex-row space-x-4">
                                <button
                                    type="button"
                                    className="z-0  inline-flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                                    onClick={submitOrder}
                                >
                                    Submit
                                </button>

                                <button
                                    type="button"
                                    className="z-0 inline-flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
                                    onClick={deletePosition}
                                >
                                    Delete
                                </button>
                            </div>
                        </div>

                        <div className="px-4 h-5/6 w-3/4">
                            { graph() }
                        </div>
                    </div>

                </form>
            </div>
        );
    }

    return (
        <div>
            { display
                ?
                displayPositionDetails()
                :
                ""
            }
        </div>
    );
}

export default PositionForm;