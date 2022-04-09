import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { take } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { DocumentUploadModalPage } from '../modals/document-upload-modal/document-upload-modal.page';
import { Generic } from '../models/generic/generic';
import { ScheduleListModel } from '../models/schedule/scheduleListModel';
import { NotificationService } from '../services/notification.service';
import { HomeService } from './home.service';

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

  constructor(private homeService: HomeService, public modalController: ModalController, private authService : AuthService, private notificationService : NotificationService) {
    this.connect();
  }

  ngOnInit() {
  }

  async connect(){
    this.notificationService.retrieveMappedObject().subscribe(
      (receivedObj: string) => {
        console.log(receivedObj);
      }
    );
  }

  ionViewWillEnter() {
    if (this.authService.isAdmin() === true) {
      this.loadFaculties();
    } else {
      this.loadUserSchedules();
    }
  }

  loadUserSchedules() {
    this.homeService.getUserSchedule().pipe(take(1)).subscribe(
      data => {
        this.schedules = data;
        this.faculties = [];
        this.title = 'Orar';
      }
    );
  }

  loadFaculties() {
    this.authService.getFacultyList().pipe(take(1)).subscribe(
      data => {
        this.faculties = data;
        this.schedules = [];
        this.sections = [];
        this.title = 'Facultati';
      }
    );
  }

  loadSections(facultyId, facultyName) {
    this.authService.getSectionList(facultyId).pipe(take(1)).subscribe(
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
    this.authService.getGroupList(sectionId).pipe(take(1)).subscribe(
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
    this.authService.getSubgroupList(groupId).pipe(take(1)).subscribe(
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
    this.homeService.getSubgroupSchedule(subgroupId).pipe(take(1)).subscribe(
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

  async foo() {
    await this.notificationService.connect(this.authService.currentUserValue().token);
  }
}

