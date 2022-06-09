export class UserDetailsModel {
    public firstName: string;
    public lastName: string;
    public emailAddress: string;
    public facultyId: number;
    public sectionId: number;
    public groupId: number;
    public subgroupId: number;

    public constructor(firstName: string, 
                       lastName: string, 
                       email: string, 
                       facultyId: number,
                       sectionId: number,
                       groupId: number,
                       subgroupId: number)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.emailAddress = email;
        this.facultyId = facultyId;
        this.sectionId = sectionId;
        this.groupId = groupId;
        this.subgroupId = subgroupId;
    }
}