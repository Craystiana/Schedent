import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { DocumentUploadModalPageRoutingModule } from './document-upload-modal-routing.module';

import { DocumentUploadModalPage } from './document-upload-modal.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DocumentUploadModalPageRoutingModule
  ],
  declarations: [DocumentUploadModalPage]
})
export class DocumentUploadModalPageModule {}
