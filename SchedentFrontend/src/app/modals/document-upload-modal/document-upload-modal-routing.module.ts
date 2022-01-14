import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DocumentUploadModalPage } from './document-upload-modal.page';

const routes: Routes = [
  {
    path: '',
    component: DocumentUploadModalPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DocumentUploadModalPageRoutingModule {}
