import { HubService } from './../services/hub.service';
import { IPurchase } from './../interfaces';
import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Location } from '@angular/common';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html',
  styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['timestampCharged', 'purchaseLines', 'wasChargeAccepted', 'subtotal'];
  dataSource = new MatTableDataSource<IPurchase>();

  purchases: IPurchase[] = [];

  $purchasesReceived: Subscription;

  constructor(private hubService: HubService, private location: Location) {
    this.$purchasesReceived = this.hubService.getPurchases().subscribe(purchases => {
      this.purchases = purchases;
      this.dataSource.data = this.purchases;
      this.dataSource.sort = this.sort;
    });
   }

  ngOnInit(): void {
    this.hubService.requestPurchases();
  }

  ngOnDestroy(): void {
    this.$purchasesReceived.unsubscribe();
  }

  back(): void {
    this.location.back();
  }
}
