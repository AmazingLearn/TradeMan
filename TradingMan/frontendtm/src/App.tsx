import React, {useEffect, useState} from 'react';
import {Route, Routes, useNavigate} from "react-router-dom";
import './App.css';
import Header from "./presentationLayer/components/header/Header";
import LoginRegister from "./presentationLayer/components/auth/LoginRegister";
import {ValidationUserDto} from "./dataLayer/api/dtos/UserDtos";
import AccountSettingsForm from "./presentationLayer/components/account/AccountSettingsForm";
import Notifications from "./presentationLayer/components/notifications/Notifications";
import {clearUserId} from "./util/Util"
import NotificationForm from "./presentationLayer/components/notifications/NotificationForm";
import Positions from "./presentationLayer/components/position/Positions";
import NotificationNew from './presentationLayer/components/notifications/NotificationNew';


function App() {

    const navigate = useNavigate();
    const [user, setUser] = useState({email: sessionStorage.getItem("user"), token: ""});

    function login(validationUser: ValidationUserDto) {
        console.log("Logging in");
        setUser({...user, email: validationUser.email});
        sessionStorage.setItem('user', validationUser.email);
        navigate("/notifications");
    }

    const logout = async () => {
        console.log("Logging out");
        setUser({
            email: "",
            token: "",
        });
        clearUserId();
        navigate("/");
    }

    if (!user.email) {
        return (<LoginRegister login={login} />)
    }

    function checkUserLoggedIn(): boolean {
        return sessionStorage.getItem('user') != null
    }

    return (
        <div className="min-h-full">
            {checkUserLoggedIn() ? <Header user={user} logout={logout}/> : "" }
            <Routes>
                <Route path="/" element={<LoginRegister login={login}/>}></Route>
                <Route path="/accountSettings" element={<AccountSettingsForm/>}/>
                <Route path="/notifications" element={<Notifications/>}/>
                <Route path="/newNotification" element={<NotificationNew/>}/>
                <Route path="/notifications/notification" element={<div><NotificationForm/></div>}/>
                <Route path="/positions/*" element={<Positions/> }/>
            </Routes>
        </div>
    );
}

export default App;
