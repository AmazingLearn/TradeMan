import React, {Dispatch, SetStateAction, useEffect, useState} from "react";
import {NotificationBasic, NotificationTrend} from "../../../dataLayer/models/Notification";
import {Link} from "react-router-dom";
import {TrashIcon} from "@heroicons/react/solid";
import {getUserAccountSettings} from "../../../dataLayer/api/UserApi";
import {removeNotification} from "../../../dataLayer/api/NotificationApi";
import NotificationDataTransfomationHelper from "../../../dataLayer/Util/NotificationDataTransfomationHelper";

export interface INotificationListProps {
	tableData : (NotificationBasic | NotificationTrend)[],
	updateTableData : (rep : NotificationBasic | NotificationTrend) => void,
	removeTableData : (rep : NotificationBasic | NotificationTrend) => void,
	setFetchdata: Dispatch<SetStateAction<boolean>>
}

/**
 * Element for displaying all notifications in a list.
 * @param props
 * @constructor
 */
function NotificationList(props: INotificationListProps) {

	const [accountSetup, setAccountSetup] = useState(false);

	useEffect(() => {
		const fetchUserAccountData = async () => {
			const res = await getUserAccountSettings();
			if (res == null)
			{
				console.log("No user account settings set.");
				setAccountSetup(false);
			}
			else
			{
				console.log("User account settings have been set.");
				setAccountSetup(true);
			}
		}

		fetchUserAccountData();
	}, []);

	const handleDeleteNotification = async (notification: NotificationBasic | NotificationTrend) => {
		props.removeTableData(notification);

		try {
			await removeNotification(NotificationDataTransfomationHelper.toNotificationDto(notification));
		} catch (err) {
			console.log(err);
		}
	}

	const handleNavigate = (notification: NotificationBasic | NotificationTrend) => {
		sessionStorage.setItem('notificationIn', JSON.stringify(notification));
	}

	function checkExpired (not: NotificationBasic | NotificationTrend): boolean {
		const date = new Date(not.expiryDate);

		if (date < new Date())
		{
			return true;
		}

		return false;
	}

	function displayNotifications() {
		return props.tableData == null
			? <></>
			: props.tableData.slice(0).reverse().map((notification, key) => (
				<tr key={key} className={`${key===0 ? "" : "border-t"} border-gray-300 bg-white`}>
					<td>
						<Link onClick={() => handleNavigate(notification)} to={"notification"}>
							<p className="px-6 pt-4 text-xl font-bold">
								{notification.name + " "}
								<strong style={{color: 'red'}}>{checkExpired(notification) ? "- EXPIRED" : ""}</strong>
								<strong style={{color: 'green'}}>{!checkExpired(notification) && notification.fullfilled ? "- FULFILLED" : ""}</strong>
							</p>
							<p className="px-6 pb-4 text-sm font-medium">{notification.symbol}</p>
						</Link>
					</td>
					<td className="px-4 w-16 py-4 text-right">
						<div className="inline-flex">
							<button className="border border-gray-300 hover:bg-gray-200 text-gray-800 font-bold py-2 px-2 rounded-r" onClick={() => handleDeleteNotification(notification)}>
								<TrashIcon className="h-5 w-5 text-red-700 group-hover:text-indigo-400" aria-hidden="true"/>
							</button>
						</div>
					</td>
				</tr>
			));
	}

	return (
		<div className="bg-gray-100 min-h-fit py-8 px-12 w-full">
			<div className="grid min-h-screen grid-flow-col">
				<div>
						{ accountSetup
							?
							<div className="pt-6 flex justify-between">
								<div>
									<h2 className="text-left text-3xl font-medium text-gray-900">Notifications</h2>
								</div>
								<Link
									className="z-0 relative flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
									to='/newNotification'
								>New notification
								</Link>
							</div>
							:
							<div>
								<h2 className="text-left text-3xl font-medium text-gray-900">Fill out account settings before creating new notifications</h2>
							</div>
						}
					<div className="w-full mt-10 space-y-6 relative overflow-x-auto rounded-lg border border-gray-300 shadow-sm">
						<table className=" min-w-full text-left text-sm text-gray-700">
							<tbody>
							{displayNotifications()}
							</tbody>
						</table>
					</div>
				</div>
			</div>
		</div>
	);
}

export default NotificationList;