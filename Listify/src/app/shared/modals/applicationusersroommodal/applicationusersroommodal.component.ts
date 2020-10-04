import { IApplicationUserRoom } from './../../../interfaces';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-applicationusersroommodal',
  template: `<mat-card class="padding-basic">
  <div class="mat-h3" mat-dialog-title>Users Online</div>
  <div mat-dialog-content class="applicationUsersRoom-container">
    <div id="applicationUserRoomList-container">
      <div *ngFor="let applicationUserRoom of data">
        <a [style.color]="applicationUserRoom.applicationUser.chatColor"
          (click)="closeButtonClick()"
          [routerLink]="['/profile', applicationUserRoom.applicationUser.username]"> {{applicationUserRoom.applicationUser.username}}</a>
      </div>
    </div>
  </div>
  <div mat-dialog-actions class="close-btn">
    <button mat-stroked-button color="primary" appearance="outline" (click)="closeButtonClick()" cdkFocusInitial [mat-dialog-close]="data">Close</button>
  </div>
</mat-card>`,
  styleUrls: ['./applicationusersroommodal.component.css']
})
export class ApplicationusersroommodalComponent implements OnInit {

  data = this.dataInstance;

  constructor(
    private dialogRef: MatDialogRef<ApplicationusersroommodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IApplicationUserRoom[]) {}

  ngOnInit(): void {
  }

  closeButtonClick(): void {
    this.dialogRef.close();
  }
}
