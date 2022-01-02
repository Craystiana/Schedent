import { UserRole } from "src/app/common/userRole";

export class UserModel {
    private _email: string;
    private _firstName: string;
    private _lastName: string;
    private _userRole: UserRole;
    private _token: string;

    constructor(JSON: any){
        this._email = JSON.email;
        this._firstName = JSON.FirstName;
        this._lastName = JSON.LastName;
        this._userRole = JSON.UserRole;
    }

    get userRole(){
        return this._userRole;
    }

    get firstName(){
        return this._firstName;
    }

    get lastName(){
        return this._lastName;
    }

    get token(){
        return this._token;
    }

    get emailAddress(){
        return this._email;
    }
}