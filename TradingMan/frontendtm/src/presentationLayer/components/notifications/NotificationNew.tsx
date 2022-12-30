import React, {useEffect, useState} from 'react'
import Select from "react-select";
import LineChart from "../graph/LineChart";
import {IgrDatePicker} from 'igniteui-react-inputs';
import {useNavigate} from "react-router-dom";
import {
    Boundary,
    boundaryOptions,
    Direction,
    directionOptions,
    NotificationType,
    typeOptions
} from '../../../dataLayer/models/Notification';
import {StockDataDto} from "../../../dataLayer/api/dtos/StockDataDtos";
import Product from "../../../dataLayer/models/Product";
import {createNotification, getAllProducts} from "../../../dataLayer/api/NotificationApi";
import {getStockDailyData} from "../../../dataLayer/api/DataApi";
import {getUserId} from "../../../util/Util";
import StockData from "../../../dataLayer/models/StockData";
import {validateNumericalInput} from "../../util/Util";
import NotificationDataTransfomationHelper from "../../../dataLayer/Util/NotificationDataTransfomationHelper";

const exchangeOptions = [
    {label: "Nysearca", value: "nysearca"},
    {label: "Nyse", value: "nyse"},
    {label: "Nasdaq", value: "nasdaq"},
    {label: "Bats", value: "bats"},
    {label: "Amex", value: "amex"},
    {label: "Arca", value: "arca"},
    {label: "Otc", value: "otc"}
]

export interface INotificationNewProps {}

/**
 * Element for creating new notifications.
 * @param props
 * @constructor
 */
function NotificationNew(props: INotificationNewProps) {

    const navigate = useNavigate();
    const [fetChedProducts, setfetChedProducts] = useState<Product[]>([]);
    const [showGraph, setShowGraph] = useState(false);
    const [graphData, setGraphData] = useState<StockDataDto[]>([]);
    const [showExpectedChange, setShowExpectedChange] = useState(false);

    const [type, setType] = useState<NotificationType>(NotificationType.unknown);

    const baseDate:Date = new Date();
    const [notificationName, setNotificationName] = useState<string>("");
    const [exchange, setExchange] = useState<string>("");
    const [selectedProductSymbol, setSelectedProductSymbol] = useState<string>("");
    const [direction, setDirection] = useState<Direction>(0);
    const [boundary, setBoundary] = useState<Boundary>(0);
    const [expected, setExpected] = useState<number>(0);
    const [date, setDate] = useState<Date>(baseDate);

    useEffect(() => {
        if (exchange !== "")
        {
            getAllProducts(exchange).then(productDtos => productDtos.map(productDto => Product.FromProductDto(productDto))).then(products => setfetChedProducts(products));
        }
    }, [exchange]);

    const handleNameChange = (name: string) => {
        setNotificationName(name);
    }

    const handleExchangeChange = (exchange: string) => {
        setExchange(exchange);
    }

    const handleProductSelect = async (product: string) => {
        const sProduct = product;
        await setSelectedProductSymbol(sProduct);

        setShowGraph(false);
        if (!await fetchGraphData(sProduct))
        {
            console.log("Unable to load graph data.");
            return;
        }

        setShowGraph(true);
    }

    const fetchGraphData = async (symbol: string): Promise<boolean> => {

        console.log("Fetching data for" + symbol);
        const data = await getStockDailyData(symbol);
        setGraphData(data);
        return true;
    }

    const handleTypeChange = (typeIn: NotificationType) => {

        setShowExpectedChange(typeIn === NotificationType.absoluteChange || typeIn === NotificationType.percentualChange)

        setType(typeIn);
    }

    const handleDirectionChange = (directionIn: Direction) => {
        setDirection(directionIn);
    }

    const handleBoundaryChange = (boundaryIn: Boundary) => {
        setBoundary(boundaryIn);
    }

    const handleExpectedValueChange = (expectedValue: string) => {

        if(validateNumericalInput(expectedValue))
        {
            setExpected(Number(expectedValue));
        }
        else
        {
            setExpected(0);
        }
    }

    const handleDateChange = (date: Date) => {
        setDate(date);
    }

    const handleNotificationCreation = async () => {
        if (type === NotificationType.unknown)
        {
            alert("Please select notification type");
            return;
        }

        if (notificationName === "")
        {
            alert("Please fill out notification name");
            return;
        }

        if (selectedProductSymbol === "")
        {
            alert("Please select a product.");
            return;
        }

        if (Number(expected) < 0)
        {
            alert("Please select a value of expected change greater than 0");
            return;
        }

        if (date <= baseDate)
        {
            alert("Please select date at least one day from now.");
            return;
        }

        // Assuming base value as close of last day.
        const base = graphData[graphData.length -1].close;
        const dateString: string = date.toString();

        if (type === NotificationType.absoluteChange || type === NotificationType.percentualChange)
        {
            await createNotification({UserId: getUserId(),
                Name: notificationName,
                Symbol: selectedProductSymbol,
                NotificationType: NotificationDataTransfomationHelper.getNotificationBasicType(type),
                ExpectedChange: expected,
                ExpiryDate: dateString,
                BaseValue: base,
                Direction: direction,
            });
        }
        else if (type === NotificationType.trendBoundaries)
        {
            await createNotification({UserId: getUserId(),
                Name: notificationName,
                Symbol: selectedProductSymbol,
                ExpectedChange: expected,
                ExpiryDate: dateString,
                BaseValue: base,
                Boundary: boundary
            });
        }

        navigate("/notifications");
    }

    function graph() {
        return (
            <LineChart data={graphData.map(item => {return StockData.fromStockDataDto(item)})} symbol={selectedProductSymbol} />
        );
    }

    function expectedChangeElem()
    {
        return (
            showExpectedChange
                ?
                <div>
                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                        Expected change
                    </label>
                    <input
                        className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full"
                        type="text"
                        onKeyPress={(event) => {
                            if (!/[0-9.]/.test(event.key)) {
                                event.preventDefault();
                            }
                        }}
                        onChange={e => handleExpectedValueChange(e.target.value)}/>
                </div>
                :
                <></>
        );
    }

    function directionOptionsElem()
    {
        if ( type === NotificationType.percentualChange || type === NotificationType.absoluteChange)
        {
            return (
                <div>
                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                        Direction
                    </label>
                    <Select
                        options={directionOptions}
                        onChange={e => handleDirectionChange(e.value)}
                        defaultValue={directionOptions[0]}
                    />
                </div>
            );
        }
    }

    function boundaryOptionsElem()
    {
        if (type === NotificationType.trendBoundaries)
        {
            return (
                <div>
                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                        Boundary
                    </label>
                    <Select
                        options={boundaryOptions}
                        onChange={e => handleBoundaryChange(e.value)}
                        defaultValue={boundaryOptions[0]}
                    />
                </div>
            );
        }
    }

    return (
        <div className="grid place-items-center fit-content h-screen">
            <form className="px-8 py-2 h-full w-3/4">
                <h2 className="py-4 mx-8 text-left text-3xl font-medium text-gray-900">New notification</h2>
                <div className="h-full flex">
                    <div className="w-1/4">
                        <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                            Name
                        </label>
                        <input className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full" type="text" placeholder="Name your notification" onChange={e => handleNameChange(e.target.value)}/>

                        <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                            Exchange
                        </label>
                        <Select
                            options={exchangeOptions}
                            onChange={e => handleExchangeChange(e.value)}
                        />

                        <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                            Product
                        </label>
                        <Select
                            options={fetChedProducts.map(product => ({label: product.Symbol + ": " + product.Name, value: product.Symbol}))}
                            onChange={e => handleProductSelect(e.value)}
                        />

                        <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                            Notification Type
                        </label>
                        <Select
                            options={typeOptions.slice(1, typeOptions.length)}
                            onChange={e => handleTypeChange(e.value)}
                            defaultValue={typeOptions[0]}

                        />

                        { directionOptionsElem() }

                        { boundaryOptionsElem() }

                        { expectedChangeElem() }

                        <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                            Expiry date
                        </label>
                        <div className="container sample">
                            <div className="container rounded border border-slate-300 outline-none focus:outline-none focus:ring w-full">
                                <IgrDatePicker showTodayButton={true} selectedValueChanged={e => {handleDateChange(e.value)}}/>
                            </div>
                        </div>

                        <div className="py-8 sm:flex sm:flex-row">
                            <button
                                type="button"
                                className="z-0  inline-flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                                onClick={handleNotificationCreation}
                            >
                                Create
                            </button>
                        </div>
                    </div>

                    <div className="px-4 h-5/6 w-3/4">
                        {showGraph ? graph() : ""}
                    </div>
                </div>
            </form>
        </div>
    );
}

export default NotificationNew;