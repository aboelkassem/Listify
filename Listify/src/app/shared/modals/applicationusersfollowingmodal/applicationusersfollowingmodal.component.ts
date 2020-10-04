import { IFollow } from './../../../interfaces';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-applicationusersfollowingmodal',
  template: `<mat-card class="padding-basic">
  <div class="mat-h3" mat-dialog-title>Users Follow</div>
  <div mat-dialog-content class="applicationUsersFollowing-container">
    <div id="applicationUserFollowingList-container">
      <div *ngFor="let follow of data">
        <a [style.color]="follow.applicationUser.chatColor"
          (click)="closeButtonClick()"
          [routerLink]="['/profile', follow.applicationUser.username]"> {{follow.applicationUser.username}}</a>
      </div>
    </div>
  </div>
  <div mat-dialog-actions class="close-btn">
    <button mat-stroked-button color="primary" appearance="outline" (click)="closeButtonClick()" cdkFocusInitial [mat-dialog-close]="data">Close</button>
  </div>
</mat-card>
`,
  styleUrls: ['./applicationusersfollowingmodal.component.css']
})
export class ApplicationusersfollowingmodalComponent implements OnInit {

  data = this.dataInstance;

  constructor(
    private dialogRef: MatDialogRef<ApplicationusersfollowingmodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IFollow[]) {}

  ngOnInit(): void {
  }

  closeButtonClick(): void {
    this.dialogRef.close();
  }

}
