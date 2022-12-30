import {
    boundaryOptions,
    directionOptions,
    NotificationBasic,
    NotificationBasicType,
    NotificationTrend,
    typeOptions
} from "../../../dataLayer/models/Notification";
import React, {useEffect, useState} from "react";
import {getStockDailyData} from "../../../dataLayer/api/DataApi";
import {StockDataDto} from "../../../dataLayer/api/dtos/StockDataDtos";
import LineChart from "../graph/LineChart";
import StockData from "../../../dataLayer/models/StockData";
import {removeNotification} from "../../../dataLayer/api/NotificationApi";
import {useNavigate} from "react-router-dom";
import NotificationDataTransfomationHelper from "../../../dataLayer/Util/NotificationDataTransfomationHelper";

export interface INotificationFormProps {}

/**
 * Element for displaying existing notifications.
 * @param props
 * @constructor
 */
function NotificationForm(props: INotificationFormProps) {

    const navigate = useNavigate();
    const [notification, setNotification] = useState<NotificationBasic | NotificationTrend>();
    const [display, setDisplay] = useState(false);
    const [graphData, setGraphData] = useState<StockDataDto[]>([]);

    const [notificationType, setNotificationType] = useState(typeOptions[0].value);

    useEffect(() => {
        const sessionStorageNotification = sessionStorage.getItem('notificationIn');
        const parsedNotification: NotificationBasic | NotificationTrend = JSON.parse(sessionStorageNotification);

        setNotification(parsedNotification);
        fetchGraphData(parsedNotification.symbol);

        setDisplay(true);
    },[]);

    const fetchGraphData = async (symbol: string) => {
        console.log("Fetching data for" + symbol);
        const data = await getStockDailyData(symbol);
        await setGraphData(data);
    }

    const handleRemove = () => {
        removeNotification(NotificationDataTransfomationHelper.toNotificationDto(notification));
        navigate("/notifications");
    }

    function graph() {
        return (
            <LineChart data={graphData.map(item => {return StockData.fromStockDataDto(item)})} symbol={notification.symbol} />
        );
    }

    function specificationElem()
    {
        if ("boundary" in notification)
        {
            return (
                <div>
                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                        Boundary
                    </label>
                    <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{boundaryOptions[notification.boundary].label}</h2>
                </div>
            );
        }
        else if ("direction" in notification)
        {
            return (
                <div>
                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                        Direction
                    </label>
                    <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{directionOptions[notification.direction].label}</h2>
                </div>
            );
        }
    }

    function getNotificationTypeName(): string {
        if ("boundary" in notification)
        {
            return 'Trend boundaries';
        }
        else if ("direction" in notification)
        {
            if (notification.notificationType == NotificationBasicType.percentualChange)
            {
                return 'Percentual change';
            }
            else if (notification.notificationType == NotificationBasicType.absoluteChange)
            {
                return 'Absolute change';
            }
        }

        console.log("Uknown notification type in notification form");
    }

    function expectedChangeElem()
    {
        if ("direction" in notification)
        {
            return (
                <div>
                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                        Expected change
                    </label>
                    <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{notification.expectedChange}</h2>
                </div>);
        }
        else if ("boundary" in notification)
        {
            // Nothing
        }
    }

    function displayNotification() {
        return (
            <div className="grid place-items-center fit-content h-screen">
                <form className="px-8 py-2 h-full w-3/4">
                    <h2 className="py-4 mx-8 text-left text-3xl font-medium text-gray-900">Notification: {notification.name}</h2>

                    <div className="h-full flex">
                        <div className="w-1/4">
                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Product
                            </label>
                            <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{notification.symbol}</h2>

                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Notification Type
                            </label>
                            <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{getNotificationTypeName()}</h2>

                            { specificationElem() }

                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Base value
                            </label>
                            <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{notification.baseValue + " USD"}</h2>

                            { expectedChangeElem() }

                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                Expiry date
                            </label>
                            <h2 className="px-3 py-2 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full">{notification.expiryDate}</h2>

                            <div className="py-8 sm:flex sm:flex-row">
                                <button
                                    type="button"
                                    className="z-0  inline-flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
                                    onClick={handleRemove}
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
            { display ?
                displayNotification()
                :
                ""
            }
        </div>
    );
}

export default NotificationForm;