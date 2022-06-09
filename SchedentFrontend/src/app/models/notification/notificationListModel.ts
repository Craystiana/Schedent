export class NotificationListModel {
    public message : string;
    public createdOn : string;

    public constructor(message: string, createdOn: string){
        this.message = message;
        this.createdOn = createdOn;
    }
}