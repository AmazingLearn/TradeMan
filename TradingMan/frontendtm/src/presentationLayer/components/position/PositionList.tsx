import React, {Dispatch, SetStateAction, useEffect, useState} from "react";
import {Link} from "react-router-dom";
import {getUserAccountSettings} from "../../../dataLayer/api/UserApi";
import Position from "../../../dataLayer/models/Position";
import {removeNotification} from "../../../dataLayer/api/NotificationApi";
import {rejectPosition} from "../../../dataLayer/api/PositionsApi";
import {TrashIcon} from "@heroicons/react/solid";

export interface INotificationListProps {
    tableData : Position[],
    positionIn: Position,
    setPositionIn: Dispatch<SetStateAction<Position>>,
    removeTableData: (rep: Position) => void
}

/**
 * Element for displaying list of available proposed positions.
 * @param props
 * @constructor
 */
function PositionList(props: INotificationListProps) {

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

    const handleNavigate = (position: Position) => {
        props.setPositionIn(position);
        sessionStorage.setItem('positionIn', JSON.stringify(position));
    }

    const handleDeletePostion = async (position: Position) => {
        props.removeTableData(position);

        try {
            await rejectPosition(position.positionId);
        } catch (err) {
            console.log(err);
        }
    }

    function displayPositions() {
        return props.tableData == null
            ? <></>
            : props.tableData.slice(0).reverse().map((position, key) => (
                <tr key={key} className={`${key===0 ? "" : "border-t"} border-gray-300 bg-white`}>
                    <td>
                        <Link onClick={() => handleNavigate(position)} to={position.positionId}>
                            <p className="px-6 pt-4 text-xl font-bold">{position.notificationName}</p>
                            <p className="px-6 pb-4 text-sm font-medium">{position.productSymbol}</p>
                        </Link>
                    </td>
                    <td className="px-4 w-16 py-4 text-right">
                        <div className="inline-flex">
                            <button className="border border-gray-300 hover:bg-gray-200 text-gray-800 font-bold py-2 px-2 rounded-r" onClick={() => handleDeletePostion(position)}>
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
                                <h2 className="text-left text-3xl font-medium text-gray-900">Proposed Positions</h2>
                            </div>
                        </div>
                        :
                        <div>
                            <h2 className="text-left text-3xl font-medium text-gray-900">Fill out account settings before viewing positions</h2>
                        </div>
                    }
                    <div className="w-full mt-10 space-y-6 relative overflow-x-auto rounded-lg border border-gray-300 shadow-sm">
                        <table className=" min-w-full text-left text-sm text-gray-700">
                            <tbody>
                            {displayPositions()}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default PositionList;