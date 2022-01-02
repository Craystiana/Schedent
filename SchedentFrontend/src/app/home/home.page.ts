import { Component, OnInit } from '@angular/core';
import { AlertController, IonRouterOutlet, ModalController } from '@ionic/angular';
import { modalController } from '@ionic/core';
import { AuthPage } from '../auth/auth.page';
import { HomeService } from './home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class HomePage implements OnInit {

  public currentModal: any;
  constructor(public modalController: ModalController) {

  }

  ngOnInit() {
  }

  async presentModal() {
    const modal = await this.modalController.create({
      component: AuthPage,
      cssClass: 'my-custom-class'
    });
    return await modal.present();
  }

}

