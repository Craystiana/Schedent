export class RegisterModel {
    public firstName: string;
    public email: string;
    public lastName: string;
    public subgroup: number;
    public password: string;

    public constructor(firstName: string, lastName: string, email: string, subgroup: number, password: string){
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.subgroup = subgroup;
        this.password = password;
    }
}