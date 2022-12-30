import React, {useEffect, useState} from "react";
import {Route, Routes} from "react-router-dom";
import {getAllPositions} from "../../../dataLayer/api/PositionsApi";
import Position from "../../../dataLayer/models/Position";
import PositionList from "./PositionList";
import PositionForm from "./PositionForm";

export interface IPositionsProps {}

/**
 * Element for displaying positions and handling basic(RUD) proposed position logic.
 * @param props
 * @constructor
 */
function Positions(props: IPositionsProps) {
    const [tableData, setTableData] = useState<Position[]>([]);
    const [positionIn, setPositionIn] = useState<Position>(JSON.parse(sessionStorage.getItem('positionIn')) || Position.fromEmpty);
    const [fetchData, setFetchdata] = useState(false);

    const fetchTableData = async () => {
        try {
            return await getAllPositions().then(item => item.map(item => Position.fromPositionDto(item)));
        }
        catch (error) {
            console.log(error.message)
        }
    }

    const updateTableData = (position : Position) => {
        console.log(position);
        setTableData(tableData => [...tableData, position]);
    }

    const removeTableData = (position : Position) => {
        console.log(position);
        setTableData(tableData.filter(item => item.positionId !== position.positionId));
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
                    <PositionList
                        tableData={tableData}
                        positionIn={positionIn}
                        setPositionIn={setPositionIn}
                        removeTableData={removeTableData}
                    />}/>
                <Route path={"/:positionId"} element={<PositionForm />}/>
            </Routes>
        </div>
    );
}

export default Positions;