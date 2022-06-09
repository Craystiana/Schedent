import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { API_URL, EDIT_PROFILE_URL, USER_DETAILS_URL } from "src/environments/environment";
import { UserDetailsModel } from "../models/user/userDetailsModel";

@Injectable({
    providedIn: 'root'
})
export class ProfileService {
  constructor(private http: HttpClient) { }

  public getUserDetails() {
    return this.http.get(API_URL + USER_DETAILS_URL).pipe(
      map((result: UserDetailsModel) => {
        return result;
      })
    );
  }

  public editProfile(model: UserDetailsModel) {
    return this.http.post(API_URL + EDIT_PROFILE_URL, model).pipe(
      map((result: boolean) => {
        return result;
      })
    );
  }
}