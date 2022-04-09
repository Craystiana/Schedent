import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AuthService } from './auth/auth.service';
import { NotificationService } from './services/notification.service';
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  constructor(private authService: AuthService, private router: Router, private notificationService : NotificationService) {
    this.authService.currentUser.subscribe(
      data => {
        //this.notificationService.connect(data.token);
      }
    )
  }

  onLogout(){
    this.authService.logout();
    this.router.navigateByUrl('/auth');
  }
}
