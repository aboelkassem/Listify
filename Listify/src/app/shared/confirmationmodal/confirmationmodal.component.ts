import { ConfirmationmodalService } from './../../services/confirmationmodal.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Inject } from '@angular/core';
import { IConfirmationModalData } from 'src/app/interfaces';

@Component({
  selector: 'app-dialog-modal',
  template: `
  <h1 mat-dialog-title>Confirm</h1>
  <div mat-dialog-content>
    <p>Are you sure you would like to {{confirmationString}}?</p>
  </div>
  <div>
    <button mat-stroked-button color="primary" appearance="outline" (click)="onNoClick()">Cancel</button>
    <button mat-stroked-button color="accent" appearance="outline" [mat-dialog-close]="data.isConfirmed" cdkFocusInitial>OK</button>
  </div>
`,
  styleUrls: ['./confirmationmodal.component.css'],
})
export class ConfirmationmodalComponent implements OnInit {

  data = this.dataInstance;
  confirmationString = this.confirmationModalService.confirmationModalMessage;

  constructor(
    private dialogRef: MatDialogRef<ConfirmationmodalComponent>,
    private confirmationModalService: ConfirmationmodalService,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IConfirmationModalData) {}

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
