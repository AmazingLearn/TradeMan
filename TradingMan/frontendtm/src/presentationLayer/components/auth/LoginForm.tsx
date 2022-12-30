import React, {useState} from 'react';
import { LockClosedIcon } from '@heroicons/react/solid';
import {Link} from "react-router-dom";
import {ValidationUserDto} from "../../../dataLayer/api/dtos/UserDtos";
import User from "../../../dataLayer/models/User";
import {loginUser} from "../../../dataLayer/api/UserApi";

export interface ILoginFormProps {
    Login: (a:any) => void
}

/**
 * Element for displaying login screen.
 * @param props
 * @constructor
 */
function LoginForm (props: ILoginFormProps) {
    const [error, setError] = useState("");
    const [details, setDetails] = useState<ValidationUserDto>(User.toValidationUser(User.fromEmpty(), ""));

    const submitHandler = async (e: React.SyntheticEvent) => {
        if (details.email == "" || details.password == "") {
            alert("Please fill out all fields details.");
            return;
        }

        e.preventDefault();
        try {
            const userId = await loginUser(details);
            if (userId !== "")
            {
                sessionStorage.setItem("userId", userId);
                await props.Login(details);
            }
            else
            {
                console.log("User validation didnt work.");
                setError("Probably wrong login details provided.");
            }
        } catch (err) {
            setError(err.message);
        }
    }

    return (
        <div>
            <div className="max-w-md w-64 space-y-8">
                <div>
                    <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">Login</h2>
                </div>
                <form className="mt-8 space-y-6" onSubmit={submitHandler}>
                    <input type="hidden" name="remember" defaultValue="true" />
                    <div className="rounded-md shadow-sm -space-y-px">
                        <div>
                            <label htmlFor="email-address" className="sr-only">
                                E-mail, please provide valid email otherwise you will have to create new account
                            </label>
                            <input
                                id="e-mnail"
                                name="email"
                                type="email"
                                autoComplete="name"
                                required
                                className="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                                placeholder="E-mail"
                                onChange={e => setDetails({...details, email: e.target.value})}
                                value={details.email}
                            />
                        </div>
                        <div>
                            <label htmlFor="password" className="sr-only">
                                Heslo
                            </label>
                            <input
                                id="password"
                                name="password"
                                type="password"
                                autoComplete="current-password"
                                required
                                className="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                                placeholder="Heslo"
                                onChange={e => setDetails({...details, password: e.target.value})}
                                value={details.password}
                            />
                        </div>
                    </div>

                    <div className="flex items-center justify-between">
                        <div className="text-sm">
                            <Link to="/register" className="font-medium text-indigo-600 hover:text-indigo-500">
                                New account
                            </Link>
                        </div>
                    </div>

                    {(error !=="") ? (
                        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative" role="alert">
                            <strong className="font-bold">{error}. </strong>
                            <span className="block sm:inline">Try again.</span>
                        </div>
                    ) : ""}

                    <div>
                        <button
                            type="submit"
                            className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                        >
					<span className="absolute left-0 inset-y-0 flex items-center pl-3">
						<LockClosedIcon className="h-5 w-5 text-indigo-500 group-hover:text-indigo-400" aria-hidden="true" />
					</span>
                            Log in
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default LoginForm;