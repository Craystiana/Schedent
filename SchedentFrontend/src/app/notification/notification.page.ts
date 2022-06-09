import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { NotificationListModel } from '../models/notification/notificationListModel';
import { NotificationService } from './notification.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.page.html',
  styleUrls: ['./notification.page.scss'],
})
export class NotificationPage implements OnInit {

  public notifications : NotificationListModel[];

  constructor(private notificationService : NotificationService, private router: Router) { }

  ngOnInit() {
  }

  ionViewWillEnter() {
    this.loadUserNotifications();
  }

  loadUserNotifications() {
    this.notificationService.getUserNotifications().pipe(take(1)).subscribe(
      data => {
        this.notifications = data;
      }
    );
  }
}
