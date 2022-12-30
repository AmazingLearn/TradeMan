import React, {useState} from 'react';
import {CreateUserDto} from "../../../dataLayer/api/dtos/UserDtos";
import {Link} from "react-router-dom";
import {loginUser, registerNewUser} from "../../../dataLayer/api/UserApi";
import User from "../../../dataLayer/models/User";

export interface IRegisterFormProps {
    Login: (a:any) => void
}

/**
 * Element for registering new account.
 * @param props
 * @constructor
 */
function RegisterForm (props: IRegisterFormProps){
    const [error, setError] = useState("");
    const [details, setDetails] = useState({
        email:"",
        password:"",
        passwordAgain:""})

    const submitHandler = async (e: React.SyntheticEvent) => {
        e.preventDefault();

        if (details.password !== details.passwordAgain) {
            setError("Passwords need to be the same.");
            return;
        }

        try {
            const newUser : CreateUserDto = {
                email: details.email,
                password: details.password
            }

            console.log(newUser);
            const res = await registerNewUser(newUser);

            if (res)
            {
                const loginDetails = User.toValidationUser(User.fromEmpty(), "");
                loginDetails.email = details.email;
                loginDetails.password = details.password;

                const userId = await loginUser(loginDetails);
                if (userId !== "")
                {
                    sessionStorage.setItem("userId", userId);
                    await props.Login(loginDetails);
                }
                else
                {
                    console.log("User validation didnt work.");
                    setError("Unable to validate freshly created account, try refreshing page and logging in.")
                }
            }
            else
            {
                console.log("Unable to validate freshly created account.");
                setError("Unable to validate freshly created account, try refreshing page and logging in.")
            }

        } catch (err) {
            setError(err);
        }
    }

    return (
        <div>
            <div className="max-w-md w-full space-y-8">
                <div>
                    <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">Create new account</h2>
                    <p className="mt-2 text-center text-sm text-gray-600">
                        Already registered?{' '}
                        <Link to="/" className="font-medium text-indigo-600 hover:text-indigo-500">
                            Log in
                        </Link>
                    </p>
                </div>
                <form className="mt-8 space-y-6" onSubmit={submitHandler}>
                    <input type="hidden" name="remember" defaultValue="true" />
                    <div>
                        <div className="block py-2">
                            <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                E-mail
                            </label>
                            <input
                                id="e-mail"
                                name="token"
                                type="email"
                                autoComplete="off"
                                required
                                className="rounded-md shadow-sm space-y-4 relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                                placeholder="E-mail"
                                onChange={e => setDetails({...details, email: e.target.value})}
                            />
                        </div>
                        <div className="block py-2">
                            <label htmlFor="password" className="block text-sm font-medium text-gray-700 py-1">
                                Password
                            </label>
                            <input
                                id="password"
                                name="password"
                                type="password"
                                autoComplete="current-password"
                                required
                                className="rounded-md shadow-sm space-y-4 relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                                placeholder="Password"
                                onChange={e => setDetails({...details, password: e.target.value})}
                            />
                        </div>
                        <div className="block py-2">
                            <label htmlFor="password" className="block text-sm font-medium text-gray-700 py-1">
                                Password again
                            </label>
                            <input
                                id="passwordAgain"
                                name="passwordAgain"
                                type="password"
                                autoComplete="current-password"
                                required
                                className="rounded-md shadow-sm space-y-4 relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                                placeholder="Pasword again"
                                onChange={e => setDetails({...details, passwordAgain: e.target.value})}
                            />
                        </div>
                    </div>

                    {(error !=="") ? (
                        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative" role="alert">
                            <strong className="font-bold">{error}</strong><br />
                            <span className="block sm:inline"> Try again.</span>
                        </div>
                    ) : ""}

                    <div>
                        <button
                            type="submit"
                            className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                        >
                            Create account
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default RegisterForm;