import { CartService } from './../services/cart.service';
import { IPurchasableItem, IPurchasableLineItem } from './../interfaces';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {

  displayedColumns: string[] = ['itemName', 'orderQuantity'];
  dataSource = new MatTableDataSource<IPurchasableLineItem>();

  purchasableLineItem: IPurchasableLineItem[] = [];

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.purchasableLineItem = this.cartService.purchase.purchase.purchaseLineItems;
    this.dataSource.data = this.purchasableLineItem;
  }
}
