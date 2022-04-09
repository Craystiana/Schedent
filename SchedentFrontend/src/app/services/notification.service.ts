import * as signalR from '@microsoft/signalr';          // import signalR
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { API_URL } from 'src/environments/environment';
import { DefaultHttpClient, HttpRequest, HttpResponse, IHttpConnectionOptions } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private connection: any;

  private sharedObj = new Subject<string>();  
  private message: string;

  constructor(private http: HttpClient) {
  }

  public async connect(token: string) {
    debugger;
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:44389/notificationHub", {
        accessTokenFactory: () => token
      } as IHttpConnectionOptions)
      .configureLogging(signalR.LogLevel.Information)
      .build()

    this.connection.onclose(async () => {
      await this.start();
    });
    this.connection.on("ReceiveOne", (text) => { this.mapReceivedMessage(text); });
    this.start();
  }

  // Strart the connection
  public async start() {
    try {
      await this.connection.start();
      console.log("connected");
    } catch (err) {
      console.log(err);
      setTimeout(() => this.start(), 5000);
    }
  }

  private mapReceivedMessage(message: string): void {
    this.message = message;
    this.sharedObj.next(this.message);
  }

  public retrieveMappedObject(): Observable<string> {
    return this.sharedObj.asObservable();
  }
}
