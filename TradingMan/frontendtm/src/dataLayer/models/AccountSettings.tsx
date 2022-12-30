import {NewUserAccountSettingsDto, UserAccountSettingsDto} from '../api/dtos/UserDtos'

/**
 * Class representing account settings, mirros backend implementation
 */
export default class AccountSettings {
    readonly alpacaApiKey: string
    readonly telegramName: string
    readonly useTelegram: boolean

    constructor(props: Partial<AccountSettings>) {
        Object.assign(this, props);
    }

    static fromEmpty(): AccountSettings {
        return new AccountSettings({
            alpacaApiKey: "",
            telegramName: "",
            useTelegram: false
        });
    }

    static toNewAccountSettings(accountSettings: AccountSettings, alpacaSecretKey: string, userId: string): NewUserAccountSettingsDto {
        return {
            userId: userId,
            alpacaApiKey: accountSettings.alpacaApiKey,
            alpacaSecretKey: alpacaSecretKey,
            telegramUsername: accountSettings.telegramName,
            useTelegram: accountSettings.useTelegram
        };
    }

    static fromAccountSettingsDto(userAccountSettingsDto: UserAccountSettingsDto): AccountSettings {
        return new AccountSettings({
            alpacaApiKey: userAccountSettingsDto.alpacaApiKey,
            telegramName: userAccountSettingsDto.telegramUsername,
            useTelegram: userAccountSettingsDto.useTelegram, });
    }
}