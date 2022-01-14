import { Component, OnInit } from '@angular/core';
import { AlertController, IonRouterOutlet, ModalController } from '@ionic/angular';
import { modalController } from '@ionic/core';
import { AuthPage } from '../auth/auth.page';
import { DocumentUploadModalPage } from '../modals/document-upload-modal/document-upload-modal.page';
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
      component: DocumentUploadModalPage,
      cssClass: 'my-custom-class'
    });
    return await modal.present();
  }

}

