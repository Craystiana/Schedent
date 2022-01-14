import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { API_URL, DOCUMENT_ADD_URL } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
  })
  export class HomeService {
    constructor(private http: HttpClient) {
    }

    addDocument(file: string) {
        return this.http.post(API_URL + DOCUMENT_ADD_URL, file).pipe(
          map((result: boolean) => {
            return result;
          })
        );
      }
  }