import { CartService } from './../services/cart.service';
import { IPurchasableItem, IPurchasableLineItem } from './../interfaces';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['itemName', 'orderQuantity'];
  dataSource = new MatTableDataSource<IPurchasableLineItem>();

  purchasableLineItem: IPurchasableLineItem[] = [];

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.purchasableLineItem = this.cartService.purchase.purchase.purchaseLineItems;
    this.dataSource.data = this.purchasableLineItem;
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
}
