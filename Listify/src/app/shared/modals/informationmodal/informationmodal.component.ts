import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IInformationModalData } from 'src/app/interfaces';

@Component({
  selector: 'app-informationmodal',
  template: `
  <h1 mat-dialog-title>{{data.title}}</h1>
  <div mat-dialog-content>
    <p>{{data.message}}</p>
  </div>
  <div mat-dialog-actions>
    <button mat-stroked-button color="primary" appearance="outline" (click)="cancel()">Ok</button>
  </div>
`,
  styleUrls: ['./informationmodal.component.css']
})
export class InformationmodalComponent implements OnInit {

  data = this.dataInstance;

  constructor(
    private dialogRef: MatDialogRef<InformationmodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IInformationModalData) {}

  ngOnInit(): void {
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
