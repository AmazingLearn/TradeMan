import React from 'react';
import {useNavigate} from "react-router-dom";

type IResetFormProps = {}

/**
 * Element representing password reset form.
 * @param props
 * @constructor
 */
function ResetForm (props: IResetFormProps){

    const navigate = useNavigate();

    const handleReturn = () => {
        navigate("/");
    }

    return (
        <div>
            <h2>Functionality not available</h2>
            <button onClick={handleReturn}>Click here to return</button>
        </div>
    );
}

export default ResetForm;