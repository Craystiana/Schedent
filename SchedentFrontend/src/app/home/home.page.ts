import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalController, IonContent } from '@ionic/angular';
import { first, take } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { DocumentUploadModalPage } from '../modals/document-upload-modal/document-upload-modal.page';
import { Generic } from '../models/generic/generic';
import { ScheduleListModel } from '../models/schedule/scheduleListModel';
import { NotificationService } from '../services/notification.service';
import { HomeService } from './home.service';
import {
  ActionPerformed,
  PushNotificationSchema,
  PushNotifications,
  Token,
} from '@capacitor/push-notifications';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class HomePage implements OnInit {

  public currentModal: any;
  public schedules: ScheduleListModel[];
  public faculties: Generic[];
  public facultyId: Generic[];
  public facultyName: string;
  public sections: Generic[];
  public sectionId: Generic[];
  public sectionName: string;
  public groups: Generic[];
  public groupId: Generic[];
  public groupName: string;
  public subgroups: Generic[];
  public subgroupId: Generic[];
  public subgroupName: string;
  public title: string;
  public shouldShowFab: boolean = false;
  @ViewChild(IonContent) content: IonContent;

  constructor(private homeService: HomeService, private router: Router, public modalController: ModalController, private authService : AuthService, private notificationService : NotificationService) {
    
   }

  ngOnInit() {
    // Request permission to use push notifications
    // iOS will prompt user and return if they granted permission or not
    // Android will just grant without prompting
    PushNotifications.requestPermissions().then(async result => {
      try {
        debugger;
      if (result.receive === 'granted') {
        // Register with Apple / Google to receive push via APNS/FCM
        await PushNotifications.register();
      } else {
        // Show some error
      }
    }catch(exception){
        console.log(exception);
    }
    });

    PushNotifications.addListener('registration', (token: Token) => {
      this.homeService.editDeviceToken(token.value);
      alert('Push registration success, token: ' + token.value);
    });

    PushNotifications.addListener('registrationError', (error: any) => {
      alert('Error on registration: ' + JSON.stringify(error));
    });

    PushNotifications.addListener(
      'pushNotificationReceived',
      (notification: PushNotificationSchema) => {
        alert('Push received: ' + JSON.stringify(notification));
      },
    );

    PushNotifications.addListener(
      'pushNotificationActionPerformed',
      (notification: ActionPerformed) => {
        alert('Push action performed: ' + JSON.stringify(notification));
      },
    );
  }

  ionViewWillEnter() {
    if (this.authService.isAdmin()) {
      this.loadFaculties();
    } else {
      this.loadUserSchedules();
    }
  }

  onScroll(event) {
    this.shouldShowFab = event.detail.scrollTop > window.innerHeight / 2;
  }

  scrollTo(elementId: string) {
    let y = document.getElementById(elementId).getBoundingClientRect().y;
    let header = document.getElementById("header").getBoundingClientRect().height;
    this.content.scrollToPoint(0, y - header, 500);
  }

  loadUserSchedules() {
    this.homeService.getUserSchedule().pipe(first()).subscribe(
      data => {
        this.schedules = data;
        this.faculties = [];
        this.title = 'Orar';
      }
    );
  }

  loadFaculties() {
    this.authService.getFacultyList().pipe(first()).subscribe(
      data => {
        this.faculties = data;
        this.schedules = [];
        this.sections = [];
        this.title = 'Facultati';
      }
    );
  }

  loadSections(facultyId, facultyName) {
    this.authService.getSectionList(facultyId).pipe(first()).subscribe(
      data => {
        this.sections = data;
        this.faculties = [];
        this.groups = [];
        this.facultyId = facultyId;
        this.facultyName = facultyName;
        this.title = this.facultyName;
      }
    )
  }

  loadGroups(sectionId, sectionName) {
    this.authService.getGroupList(sectionId).pipe(first()).subscribe(
      data => {
        this.groups = data;
        this.sections = [];
        this.subgroups = [];
        this.sectionId = sectionId;
        this.sectionName = sectionName;
        this.title = this.sectionName;
      }
    )
  }

  loadSubgroups(groupId, groupName) {
    this.authService.getSubgroupList(groupId).pipe(first()).subscribe(
      data => {
        this.subgroups = data;
        this.groups = [];
        this.schedules = [];
        this.groupId = groupId;
        this.groupName = groupName;
        this.title = this.groupName;
      }
    )
  }

  loadSubgroupSchedules(subgroupId, subgroupName) {
    this.homeService.getSubgroupSchedule(subgroupId).pipe(first()).subscribe(
      data => {
        this.schedules = data;
        this.subgroups = [];
        this.subgroupId = subgroupId;
        this.subgroupName = subgroupName;
        this.title = this.groupName + this.subgroupName;
      }
    )
  }

  async presentModal() {
    const modal = await this.modalController.create({
      component: DocumentUploadModalPage,
      cssClass: 'my-custom-class'
    });
    return await modal.present();
  }
}

