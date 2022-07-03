import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { first, take } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { Generic } from '../models/generic/generic';
import { UserDetailsModel } from '../models/user/userDetailsModel';
import { ProfileService } from './profile.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage implements OnInit {
  public profile: UserDetailsModel;
  private faculties: Generic[];
  private sections: Generic[];
  private groups: Generic[];
  private subgroups: Generic[];
  private subgroupId: number; 
  isLoading: boolean;

  constructor(private profileService : ProfileService, private authService : AuthService, private router : Router, private toastCtrl : ToastController) {  }

  ngOnInit() {
    this.loadFaculties();
    this.loadUserDetails();
  }

  loadUserDetails() {
    if (!this.authService.isAdmin() && !this.authService.isProfessor()) {
      this.profileService.getUserDetails().pipe(first()).subscribe(
        data => {
          this.authService.getSectionList(data.facultyId).pipe(take(1)).subscribe(
            data1 => {
              this.sections = data1;
              this.authService.getGroupList(data.sectionId).pipe(take(1)).subscribe(
                data2 => {
                  this.groups = data2;
                  this.authService.getSubgroupList(data.groupId).pipe(take(1)).subscribe(
                    data3 => {
                      this.subgroups = data3;
                      this.subgroupId = data.subgroupId;
                      this.profile = data;
                    }
                  );
                }
              );
            }
          );
        }
      );
    } else {
      this.profileService.getUserDetails().pipe(first()).subscribe(
        data => {
          this.profile = data;
        }
      );
    }
  }

  loadFaculties() {
    this.authService.getFacultyList().pipe(take(1)).subscribe(
      data => {
        this.faculties = data;
      }
    );
  }

  onFacultyChange(facultyId) {
    this.authService.getSectionList(facultyId).pipe(take(1)).subscribe(
      data => {
        this.sections = data;
        this.profile.sectionId = null;
        this.profile.groupId = null;
        this.profile.subgroupId = null;
      }
    );
  }

  onSectionChange(sectionId: number) {
    this.authService.getGroupList(sectionId).pipe(take(1)).subscribe(
      data => {
        this.groups = data;
        this.profile.groupId = null;
        this.profile.subgroupId = null;
      }
    );
  }

  onGroupChange(groupId: number) {
    this.authService.getSubgroupList(groupId).pipe(take(1)).subscribe(
      data => {
        this.subgroups = data;
        this.profile.subgroupId = null;
      }
    );
  }

  onSubgroupChange(subgroupId: number) {
    this.subgroupId = subgroupId;
  }

  onProfileEdit(profileForm: NgForm){
    this.isLoading = true;
    var model = new UserDetailsModel(profileForm.value.firstName,
                                     profileForm.value.lastName,
                                     profileForm.value.emailAddress,
                                     profileForm.value.faculty,
                                     profileForm.value.section,
                                     profileForm.value.group,
                                     profileForm.value.subgroup
                                    );
                          
    this.profileService.editProfile(model).pipe(first()).subscribe(
      data =>{
        if(data==true){
          this.router.navigateByUrl('/auth/profile');
          this.toastCtrl.create({
            message: 'Profilul a fost editat cu succes!',
            duration: 5000,
            position: 'bottom',
            color: 'success',
            buttons: ['Dismiss']
          }).then((el) => el.present());
        }
        
        this.isLoading = false;
      },
      error => {
        this.toastCtrl.create({
          message: 'Editarea profilului a eșuat!',
          duration: 5000,
          position: 'bottom',
          color: 'danger',
          buttons: ['Dismiss']
        }).then((el) => el.present());
        
        this.isLoading = false;
      }
    )
  }

  onLogout(){
    this.authService.logout();
    this.router.navigateByUrl('/auth');
  }
}
