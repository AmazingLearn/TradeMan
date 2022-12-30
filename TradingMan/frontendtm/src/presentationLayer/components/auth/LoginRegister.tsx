import React from 'react';
import {Routes, Route} from "react-router-dom";
import LoginForm from "./LoginForm";
import RegisterForm from "./RegisterForm";
import ResetForm from "./ResetForm";

type ILoginRegisterProps = {
    login: (a : any) => void
}

/**
 * Function that returns login screen with routes to register new account and reset password.
 * @param props
 * @constructor
 */
function LoginRegister (props: ILoginRegisterProps){
    return (
        <div className="min-h-full flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
            <Routes>
                <Route path="/" element={<LoginForm Login={props.login}/>}/>
                <Route path="/register" element={<RegisterForm Login={props.login}/>}/>
                <Route path="/password-reset" element={<ResetForm />}/>
            </Routes>
        </div>
    );
}

export default LoginRegister;