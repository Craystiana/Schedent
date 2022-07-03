import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs/internal/Observable';
import { last, map } from 'rxjs/operators';
import { API_URL, FACULTY_LIST_URL, GROUP_LIST_URL, LOGIN_URL, REGISTER_URL, SECTION_LIST_URL, SUBGROUP_LIST_URL } from 'src/environments/environment';
import { UserRole } from '../common/userRole';
import { RegisterModel } from '../models/user/registerModel';
import { UserModel } from '../models/user/userModel';
import { Generic } from '../models/generic/generic';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<UserModel>;
  public currentUser: Observable<UserModel>;
  static currentUser: any;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<UserModel>(JSON.parse(sessionStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public getFacultyList() {
    return this.http.get(API_URL + FACULTY_LIST_URL).pipe(
      map((result : Generic[]) =>{
        return result;
      })
    );
  }

  public getSectionList(facultyId: number) {
    return this.http.get(API_URL + SECTION_LIST_URL + facultyId).pipe(
      map((result : Generic[]) =>{
        return result;
      })
    );
  }

  public getGroupList(sectionId: number) {
    return this.http.get(API_URL + GROUP_LIST_URL + sectionId).pipe(
      map((result : Generic[]) =>{
        return result;
      })
    );
  }

  public getSubgroupList(groupId: number) {
    return this.http.get(API_URL + SUBGROUP_LIST_URL + groupId).pipe(
      map((result : Generic[]) =>{
        return result;
      })
    );
  }

  public get currentUserName(): String {
    if (this.currentUserSubject.value != null) {
      var user = this.currentUserSubject.value;
      return user.lastName + ' ' + user.firstName;
    }
    else {
      return null;
    }
  }

  public get currentUserEmail(): String {
    if (this.currentUserSubject.value != null) {
      var user = this.currentUserSubject.value;
      return user.emailAddress;
    }
    else {
      return null;
    }
  }

  public currentUserValue(): UserModel {
    return this.currentUserSubject.value;
  }

  public isAuthenticated(): boolean {
    return this.currentUserSubject.value != null;
  }

  public isAdmin(): boolean {
    if (this.isAuthenticated()) {
      return this.currentUserSubject.value.userRole === UserRole.Admin;
    } else {
      return false;
    }
  }

  public isProfessor(): boolean {
    if (this.isAuthenticated() === true) {
      return this.currentUserSubject.value.userRole === UserRole.Professor;
    } else {
      return false;
    }
  }

  public isStudent(): boolean {
    if (this.isAuthenticated() === true) {
      return this.currentUserSubject.value.userRole === UserRole.Student;
    } else {
      return false;
    }
  }

  login(email: string, password: String) {
    return this.http.post(API_URL + LOGIN_URL, { email, password }).pipe(
      map((user: UserModel) => {
        sessionStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  logout() {
    sessionStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  register(model: RegisterModel) {
    return this.http.post(API_URL + REGISTER_URL, model).pipe(
      map((result: boolean) => {
        return result;
      })
    );
  }

  // public getProfile(){
  //   return this.http.get(API_URL + PROFILE_URL).pipe(
  //     map((data : Profile) => {
  //       return data;
  //     })
  //   );
  // }

  // editProfile(model: Profile){

  //   return this.http.post(API_URL + PROFILE_URL, model).pipe(
  //     map((result: boolean) =>{
  //       return result;
  //     })
  //   );
  // }
}