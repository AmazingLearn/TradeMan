import React, {useEffect, useState} from 'react';
import AccountSettings from "../../../dataLayer/models/AccountSettings";
import {getUserAccountSettings} from "../../../dataLayer/api/UserApi";
import AccountSettingsNew from "./AccountSettingsNew";
import {UserAccountSettingsDto} from "../../../dataLayer/api/dtos/UserDtos";

export interface IAccountFormProps {}

/**
 * Element for displaying account settins.
 * @param props
 * @constructor
 */
function AccountSettingsForm (props: IAccountFormProps) {

    const [newSettings, setNewSettings] = useState(false);
    const [settings, setSettings] = useState<AccountSettings>({alpacaApiKey: "Not available", telegramName: "Not available", useTelegram: false});
    const [reload, setReload] = useState(false);

    useEffect(() => {
        let res: UserAccountSettingsDto = null;
        const fetchData = async () => {
            const res = await getUserAccountSettings();
            if (res != null)
            {
                setSettings(AccountSettings.fromAccountSettingsDto(res));
            }
        }

        fetchData();

        console.log("Res is:" + res);
    },[])

    useEffect(() => {
        if (reload === true)
        {
            window.location.reload();
            setReload(false);
        }
    }, [reload]);

    const handleModifyAccountSettings = () => {
        setNewSettings(true);
    }

    return (
        <div className="flex h-screen">
            <div className="m-auto space-y-4">
                {newSettings ?
                    <AccountSettingsNew
                        viewDialog={newSettings}
                        setViewDialog={setNewSettings}
                        existngSettings={settings}
                    />
                    :
                    ""
                }
                <h2><strong>Instructions -- nejak toto lepsie sformatovat ...</strong></h2>
                <p>Before changing account settings, make sure to</p>
                <p>send any message to telegram bot account</p>
                <p> called 'Trade Man' with username '@TM536Bot'.</p>
                <p>Otherwise you wont be able to receive </p>
                <p>telegram notifications.</p>
                <h2 className="pt-4" ><strong>Account settings -- toto dat asi nejak do stredu a ten instr nabok</strong></h2>
                <h3><strong>Alpaca Api Key:</strong> {settings.alpacaApiKey}</h3>
                <h3><strong>Telegram Name:</strong> {settings.telegramName}</h3>
                <button onClick={handleModifyAccountSettings} className="z-0 relative flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                    New settings
                </button>
            </div>
        </div>
    );
}

export default AccountSettingsForm;