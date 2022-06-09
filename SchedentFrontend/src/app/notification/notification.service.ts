import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { API_URL, NOTIFICATION_LIST_URL } from "src/environments/environment";
import { NotificationListModel } from "../models/notification/notificationListModel";

@Injectable({
    providedIn: 'root'
})
export class NotificationService {
  constructor(private http: HttpClient) {
  }

  public getUserNotifications(){
    return this.http.get(API_URL + NOTIFICATION_LIST_URL).pipe(
      map((result: NotificationListModel[]) =>{
        return result;
      })
    );
  }
}