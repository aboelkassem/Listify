import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Inject } from '@angular/core';
import { IConfirmationModalData } from 'src/app/interfaces';

@Component({
  selector: 'app-dialog-modal',
  template: `
  <mat-card class="padding-simple">
    <h1 mat-dialog-title>{{data.title}}</h1>
    <div mat-dialog-content>
      <p>{{data.message}}</p>
    </div>
    <div mat-dialog-actions>
      <button mat-stroked-button color="primary" appearance="outline" (click)="onNoClick()">{{data.cancelMessage}}</button>
      <button mat-stroked-button color="accent" appearance="outline" [mat-dialog-close]="data.isConfirmed" cdkFocusInitial>{{data.confirmMessage}}</button>
    </div>
  </mat-card>
`,
  styleUrls: ['./confirmationmodal.component.css'],
})
export class ConfirmationmodalComponent implements OnInit {

  data = this.dataInstance;

  constructor(
    private dialogRef: MatDialogRef<ConfirmationmodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IConfirmationModalData) {}

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
