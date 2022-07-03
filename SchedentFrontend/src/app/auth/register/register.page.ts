import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { first, take } from 'rxjs/operators';
import { Generic } from 'src/app/models/generic/generic';
import { RegisterModel } from 'src/app/models/user/registerModel';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit {

  private faculties: Generic[];
  private sections: Generic[];
  private groups: Generic[];
  private subgroups: Generic[];
  private subgroupId: number; 
  public isLoading: boolean;
  constructor(private authService: AuthService, private router: Router, private toastCtrl: ToastController) { }

  ngOnInit() { }

  ionViewWillEnter() {
    this.loadData();
  }

  loadData(){
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
      }
    );
  }

  onSectionChange(sectionId: number) {
    this.authService.getGroupList(sectionId).pipe(take(1)).subscribe(
      data => {
        this.groups = data;
      }
    );
  }

  onGroupChange(groupId: number) {
    this.authService.getSubgroupList(groupId).pipe(take(1)).subscribe(
      data => {
        this.subgroups = data;
      }
    );
  }

  onSubgroupChange(subgroupId: number) {
    this.subgroupId = subgroupId;
  }

  onRegister(registerForm: NgForm) {
    this.isLoading = true;
    var model = new RegisterModel(registerForm.value.firstName,
                                  registerForm.value.lastName,
                                  registerForm.value.email,
                                  this.subgroupId,
                                  registerForm.value.password);

    this.authService.register(model).pipe(first()).subscribe(
      data => {
        if (data == true) {
          this.router.navigateByUrl('/auth');
          this.toastCtrl.create({
            message: 'Înregistrare reușită. Conectează-te.',
            duration: 5000,
            position: 'bottom',
            color: 'success',
            buttons: ['Dismiss']
          }).then((el) => el.present());
        }
        else {
          this.toastCtrl.create({
            message: 'Înregistrare eșuată',
            duration: 5000,
            position: 'bottom',
            color: 'danger',
            buttons: ['Dismiss']
          }).then((el) => el.present());
        }
        this.isLoading = false;
      },
      error => {
        this.toastCtrl.create({
          message: 'Înregistrare eșuată',
          duration: 5000,
          position: 'bottom',
          color: 'danger',
          buttons: ['Dismiss']
        }).then((el) => el.present());

        this.isLoading = false;
      }
    )
  }
}
