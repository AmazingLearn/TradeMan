import React, {useEffect, useState} from "react";
import {Route, Routes} from "react-router-dom";
import NotificationList from "./NotificationList";
import NotificationForm from "./NotificationForm";
import {getNotifications} from "../../../dataLayer/api/NotificationApi";
import {NotificationBasic, NotificationTrend} from "../../../dataLayer/models/Notification";
import NotificationDataTransfomationHelper from "../../../dataLayer/Util/NotificationDataTransfomationHelper";

export interface INotificationProps {}

/**
 * Element for displaying notifications and handling basic(RUD) notification related logic.
 * @param props
 * @constructor
 */
function Notifications (props: INotificationProps) {

    const [tableData, setTableData] = useState<(NotificationBasic|NotificationTrend)[]>([]);
    const [fetchData, setFetchdata] = useState(false);

    const fetchTableData = async () => {
        try {
            return await getNotifications().then(item => item.map(item => NotificationDataTransfomationHelper.fromNotificationDto(item)));
        }
        catch (error) {
            console.log(error.message)
        }
    }

    const updateTableData = (notification : NotificationBasic | NotificationTrend) => {
        console.log(notification);
        setTableData(tableData => [...tableData, notification]);
    }

    const removeTableData = (notification : NotificationBasic | NotificationTrend) => {
        console.log(notification);
        setTableData(tableData.filter(item => item.notificationId !== notification.notificationId));
    }

    useEffect(() => {
        fetchTableData().then(items => {
            setTableData(items);
            console.log(items);
            setFetchdata(false);
        });
    }, [fetchData, true]);

    return (
        <div className="grid min-h-screen grid-flow-col">
            <Routes>
                <Route path="/" element={
                    <NotificationList
                        tableData={tableData}
                        updateTableData={updateTableData}
                        removeTableData={removeTableData}
                        setFetchdata={setFetchdata}
                    />}/>
                <Route path={"/notification"} element={<NotificationForm />}/>
            </Routes>
        </div>
    );
}

export default Notifications;