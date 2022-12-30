import {CreateUserDto, NewUserAccountSettingsDto, UserAccountSettingsDto, ValidationUserDto} from "./dtos/UserDtos";
import {API_URL} from "../../util/config";
import {getUserId} from "../../util/Util";

/**
 * Calls endpoint to register a new user into the application.
 * @param newUser
 */
export async function registerNewUser(newUser: CreateUserDto) : Promise<boolean> {
    console.log("Registering new user.");

    const res = await fetch(`${API_URL}/Users/CreateUser`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(newUser),
    });

    if (res.status != 200) {
        alert(await res.text());
        return false;
    }

    return res.ok;
}

/**
 * Calls endpoint to login user - if login details are correct, userId gets returned.
 * @param validationUser
 */
export async function loginUser(validationUser: ValidationUserDto): Promise<string> {

    console.log("Loggin in user.");

    const res = await fetch(`${API_URL}/Users/LoginUser`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(validationUser),
    });

    if (await res.status != 200) {
        alert(await res.text());
        return "";
    }

    return await res.json();
}

/**
 * Calls endpoint to get userAccount settings for logged in user.
 */
export async function getUserAccountSettings():Promise<UserAccountSettingsDto> {

    console.log("Getting user account settings");

    const res = await fetch(`${API_URL}/Users/GetAccountSettings/${getUserId()}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    if (res.status != 200) {
        alert(await res.text());
        return null;
    }

    return await res.json();
}

/**
 * Calls endpoint to sat new user account settings for logged in user
 * @param newUserAccountSettingsDto
 */
export async function setUserAccountSettings(newUserAccountSettingsDto: NewUserAccountSettingsDto):Promise<boolean> {

    console.log("Saving new user account settings.");

    const res = await fetch(`${API_URL}/Users/SetAccountSettings`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(newUserAccountSettingsDto),
    });

    if (res.status != 200) {
        alert(await res.text());
        return false;
    }

    return res.ok;
}