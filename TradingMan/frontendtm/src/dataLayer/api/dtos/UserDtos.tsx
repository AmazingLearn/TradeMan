export interface CreateUserDto {
    email: string,
    password: string
}

export interface ValidationUserDto {
    userId: string,
    email: string,
    password: string
}

export interface UserAccountSettingsDto {
    alpacaApiKey: string,
    telegramUsername: string
    useTelegram: boolean
}

export interface NewUserAccountSettingsDto {
    userId: string,
    alpacaApiKey: string,
    alpacaSecretKey: string,
    telegramUsername: string
    useTelegram: boolean
}