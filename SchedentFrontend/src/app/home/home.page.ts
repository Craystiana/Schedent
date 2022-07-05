import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ModalController, IonContent } from '@ionic/angular';
import { first } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { DocumentUploadModalPage } from '../modals/document-upload-modal/document-upload-modal.page';
import { Generic } from '../models/generic/generic';
import { ScheduleListModel } from '../models/schedule/scheduleListModel';
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
  public activeTab: number;
  @ViewChild(IonContent) content: IonContent;

  constructor(private homeService: HomeService, private router: Router, public modalController: ModalController, private authService : AuthService) {
    
   }

  ngOnInit() {
    PushNotifications.requestPermissions().then(async result => {
      try {
        if (result.receive === 'granted') {
          await PushNotifications.register();
        }
      }catch(exception){
          console.log(exception);
      }
    });

    PushNotifications.addListener('registration', (token: Token) => {
      this.homeService.editDeviceToken(token.value).pipe(first()).subscribe();
    });

    PushNotifications.addListener('registrationError', (error: any) => {
      alert('Error on registration: ' + JSON.stringify(error));
    });

    PushNotifications.addListener('pushNotificationReceived',
      (notification: PushNotificationSchema) => {
        alert('Push received: ' + JSON.stringify(notification));
      },
    );

    PushNotifications.addListener('pushNotificationActionPerformed',
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
        this.activeTab = 5;
      }
    );
  }

  loadFaculties() {
    this.authService.getFacultyList().pipe(first()).subscribe(
      data => {
        this.faculties = data;
        this.schedules = [];
        this.sections = [];
        this.title = 'Facultăți';
        this.activeTab = 1;
      }
    );
  }

  loadSections(facultyId, facultyName) {
    this.facultyId = facultyId;
    this.facultyName = facultyName;
    this.authService.getSectionList(facultyId).pipe(first()).subscribe(
      data => {
        this.sections = data;
        this.faculties = [];
        this.groups = [];
        this.title = this.facultyName;
        this.activeTab = 2;
      }
    )
  }

  loadGroups(sectionId, sectionName) {
    this.sectionId = sectionId;
    this.sectionName = sectionName;
    this.authService.getGroupList(sectionId).pipe(first()).subscribe(
      data => {
        this.groups = data;
        this.sections = [];
        this.subgroups = [];
        this.title = this.sectionName;
        this.activeTab = 3;
      }
    )
  }

  loadSubgroups(groupId, groupName) {
    this.groupId = groupId;
    this.groupName = groupName;
    this.authService.getSubgroupList(groupId).pipe(first()).subscribe(
      data => {
        this.subgroups = data;
        this.groups = [];
        this.schedules = [];
        this.title = this.groupName;
        this.activeTab = 4;
      }
    )
  }

  loadSubgroupSchedules(subgroupId, subgroupName) {
    this.subgroupId = subgroupId;
    this.subgroupName = subgroupName;
    this.homeService.getSubgroupSchedule(subgroupId).pipe(first()).subscribe(
      data => {
        this.schedules = data;
        this.subgroups = [];
        this.title = this.groupName + this.subgroupName;
        this.activeTab = 5;
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

