import {ValidationUserDto} from "../api/dtos/UserDtos"

/**
 * Class representing application user, mirrors backend implementation.
 */
export default class User {
    readonly email: string
    readonly password: string

    constructor(props: Partial<User>) {
        Object.assign(this, props);
    }

    static fromEmpty(): User {
        return new User({
            email: "",
            password: "",
        });
    }

    static toValidationUser(user: User, userId: string): ValidationUserDto {
        return {
            userId: userId,
            email: user.email,
            password: user.password,
        };
    }
}