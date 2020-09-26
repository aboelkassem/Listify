import { IInputModalData } from './../../../interfaces';
import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-inputmodal',
  template: `
    <h1 mat-dialog-title>{{data.title}}</h1>
    <div mat-dialog-content>
      <p>{{data.message}}</p>
    </div>
    <mat-form-field>
        <input matInput id="data.data" (keyup.enter)="confirmButtonClick(data.data)" name="data" [placeholder]="data.placeholder" [(ngModel)]="data.data">
    </mat-form-field>
    <div mat-dialog-actions>
      <button mat-stroked-button color="primary" appearance="outline" (click)="cancel()">Cancel</button>
      <button mat-stroked-button color="accent" appearance="outline" [mat-dialog-close]="data.data" cdkFocusInitial>OK</button>
    </div>
  `,
  styleUrls: ['./inputmodal.component.css']
})
export class InputmodalComponent implements OnInit {

  data = this.dataInstance;

  constructor(
    private dialogRef: MatDialogRef<InputmodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IInputModalData) {}

  ngOnInit(): void {
  }

  cancel(): void {
    this.dialogRef.close();
  }

  confirmButtonClick(data: string): void {
    this.dialogRef.close(data);
  }
}
