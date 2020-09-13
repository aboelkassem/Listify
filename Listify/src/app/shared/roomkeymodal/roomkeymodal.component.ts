import { IRoomKeyModalData } from './../../interfaces';
import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-roomkeymodal',
  template: `
    <h1 mat-dialog-title>Confirm</h1>
    <div mat-dialog-content>
      <p>Room Locked, Please enter the room Key!</p>
    </div>
    <mat-form-field>
        <input matInput id="roomKey" (keyup.enter)="confirmButtonClick(data.roomKey)" name="roomKey" placeholder="Enter your Room Key" [(ngModel)]="data.roomKey">
    </mat-form-field>
    <div mat-dialog-actions>
      <button mat-stroked-button color="primary" appearance="outline" (click)="cancel()">Cancel</button>
      <button mat-stroked-button color="accent" appearance="outline" [mat-dialog-close]="data.roomKey" cdkFocusInitial>OK</button>
    </div>
  `,
  styleUrls: ['./roomkeymodal.component.css']
})
export class RoomkeymodalComponent implements OnInit {

  data = this.dataInstance;

  constructor(
    private dialogRef: MatDialogRef<RoomkeymodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IRoomKeyModalData) {}

  ngOnInit(): void {
  }

  cancel(): void {
    this.dialogRef.close();
  }

  confirmButtonClick(roomKey: string): void {
    this.dialogRef.close(roomKey);
  }
}
