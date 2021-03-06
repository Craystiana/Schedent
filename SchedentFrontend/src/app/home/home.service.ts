import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { API_URL, DOCUMENT_ADD_URL, EDIT_DEVICE_TOKEN, SUBGROUP_SCHEDULE_URL, USER_SCHEDULE_URL } from "src/environments/environment";
import { ScheduleListModel } from "../models/schedule/scheduleListModel";

@Injectable({
    providedIn: 'root'
  })
  export class HomeService {
    constructor(private http: HttpClient) {
    }

    public getUserSchedule(){
      return this.http.get(API_URL + USER_SCHEDULE_URL).pipe(
        map((result: ScheduleListModel[]) =>{
          return result;
        })
      );
    }

    public getSubgroupSchedule(subgroupId) {
      return this.http.get(API_URL + SUBGROUP_SCHEDULE_URL + subgroupId).pipe(
        map((result: ScheduleListModel[]) =>{
          return result;
        })
      );
    }

    addDocument(file: string) {
      return this.http.post(API_URL + DOCUMENT_ADD_URL, file).pipe(
        map((result: boolean) => {
          return result;
        })
      );
    }

    editDeviceToken(token: string) {
      return this.http.put(API_URL + EDIT_DEVICE_TOKEN + token, null).pipe(
        map((result: boolean) => {
          return result;
        })
      )
    }
  }