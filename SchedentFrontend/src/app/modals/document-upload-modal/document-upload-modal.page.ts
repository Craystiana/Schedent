import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ModalController, ToastController } from '@ionic/angular';
import { first, map } from 'rxjs/operators';
import { API_URL, DOCUMENT_ADD_URL } from 'src/environments/environment';

@Component({
  selector: 'app-document-upload-modal',
  templateUrl: './document-upload-modal.page.html',
  styleUrls: ['./document-upload-modal.page.scss'],
})
export class DocumentUploadModalPage implements OnInit {
  private isLoading: boolean;
  private pictureBase64: string;
  private isDocumentLoaded = false;

  constructor(private http: HttpClient, private toastCtrl: ToastController, private modalController: ModalController) { }

  ngOnInit() {
  }

  addTimeTable(file: string) {
    return this.http.post(API_URL + DOCUMENT_ADD_URL, { file: file }).pipe(
      map((result: boolean) => {
        return result;
      })
    );
  }

  onEdit() {
    this.isLoading = true;
    this.addTimeTable(this.pictureBase64).pipe(first()).subscribe(
      data => {
        if (data === true) {
          this.toastCtrl.create({
            message: 'Orar adăugat cu succes.',
            duration: 5000,
            position: 'bottom',
            color: 'success',
            buttons: ['Dismiss']
          }).then((el) => el.present());
          this.modalController.dismiss();
        }
        else {
          this.toastCtrl.create({
            message: 'Eroare. Vă rugăm reîncercați.',
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
          message: 'Eroare. Vă rugăm reîncercați.',
          duration: 5000,
          position: 'bottom',
          color: 'danger',
          buttons: ['Dismiss']
        }).then((el) => el.present());

        this.isLoading = false;
      }
    )
  }

  onDocumentUpload(files) {
    const reader = new FileReader();

    reader.readAsDataURL(files.item(0));
    this.isDocumentLoaded = false;

    reader.onload = () => {
      this.pictureBase64 = reader.result.toString().split('base64,').pop();
      this.isDocumentLoaded = true;
    }
  }

}
