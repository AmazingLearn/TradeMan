import {Dispatch, SetStateAction, Fragment, useRef, useState, useEffect} from "react";
import { Dialog, Transition } from '@headlessui/react'
import AccountSettings from "../../../dataLayer/models/AccountSettings";
import {setUserAccountSettings} from "../../../dataLayer/api/UserApi";
import {getUserId} from "../../../util/Util";

export interface IAccountSettingsNewProps {
    viewDialog: boolean,
    setViewDialog: Dispatch<SetStateAction<boolean>>,
    existngSettings: AccountSettings
}

/**
 * Element for creating new account settings.
 * @param props
 * @constructor
 */
function AccountSettingsNew (props: IAccountSettingsNewProps) {

    const [alpacaApiKey, setalpacaApiKey] = useState(props.existngSettings.alpacaApiKey);
    const [alpacaSecretKey, setAlpacaSecretKey] = useState("");
    const [telegramName, setTelegramName] = useState(props.existngSettings.telegramName);
    const [useTelegram, setUseTelegram] = useState(false);
    const cancelButtonRef = useRef(null);

    const submitHandler = async (e: React.SyntheticEvent) => {
        e.preventDefault();
    };

    const handleUseTelegramChange = () => {
        setUseTelegram(!useTelegram);
    }

    const handleSettingsCreation = async () => {
        console.log("Handling settings creations");
        const userId = getUserId();

        try {
            const res = await setUserAccountSettings({userId: userId, alpacaApiKey: alpacaApiKey, alpacaSecretKey: alpacaSecretKey, telegramUsername: telegramName, useTelegram: useTelegram});
            if (!res)
            {
                alert("Cannot validate provided settings. Make sure you have provided correct values and messaged telegram bot.");
            }

        }
        catch(error)
        {
            reportError(error);
        }

        props.setViewDialog(false);
    }

    return (
        <Transition.Root show={props.viewDialog} as={Fragment}>
            <Dialog as="div" className="fixed z-10 inset-0 overflow-y-auto" initialFocus={cancelButtonRef} onClose={props.setViewDialog}>
                <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0"
                        enterTo="opacity-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <Dialog.Overlay className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
                    </Transition.Child>

                    {/* This element is to trick the browser into centering the modal contents. */}
                    <span className="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">
            &#8203;
          </span>
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                        enterTo="opacity-100 translate-y-0 sm:scale-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100 translate-y-0 sm:scale-100"
                        leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                    >
                        <div className="relative inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:m-8 sm:px-6 sm:py-4 sm:align-middle sm:max-w-2xl sm:w-full">
                            <h2 className="mt-8 mx-8 text-left text-3xl font-medium text-gray-900">New Account Settings</h2>
                            <form className="pt-6 px-8 py-2" onSubmit={submitHandler}>
                                <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                    Alpaca Api Key
                                </label>
                                <input className="px-3 py-4 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full" id="alpacaApi" type="text" placeholder={alpacaApiKey} onChange={e => {setalpacaApiKey(e.target.value)}}/>

                                <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                    Alpaca Secret Key
                                </label>
                                <input className="px-3 py-4 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full" id="alpacaKey" type="text" placeholder={alpacaSecretKey} onChange={e => {setAlpacaSecretKey(e.target.value)}}/>

                                <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                    Telegram Username - without leading '@'
                                </label>
                                <input className="px-3 py-4 placeholder-slate-300 text-slate-600 relative bg-white rounded text-base border border-slate-300 outline-none focus:outline-none focus:ring w-full" id="telegramName" type="text" placeholder={telegramName} onChange={e => {setTelegramName(e.target.value)}}/>

                                <div className="items-start">
                                    <label htmlFor="user-name" className="block text-sm font-medium text-gray-700 py-1">
                                        Use telegram
                                    </label>
                                    <input type="checkbox" onChange={handleUseTelegramChange}></input>
                                </div>

                                <div className="py-8 sm:flex sm:flex-row">
                                    <button
                                        type="button"
                                        className="z-0  inline-flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                                        onClick={handleSettingsCreation}
                                    >
                                        Confirm
                                    </button>
                                </div>


                            </form>
                        </div>
                    </Transition.Child>
                </div>
            </Dialog>
        </Transition.Root>
    );
}

export default AccountSettingsNew;