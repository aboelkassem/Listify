import { CartService } from './../services/cart.service';
import { IPurchasableItem } from './../interfaces';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {

  displayedColumns: string[] = ['itemName', 'orderQuantity'];
  dataSource = new MatTableDataSource<IPurchasableItem>();

  purchasableLineItem: IPurchasableItem[] = [];

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.purchasableLineItem = this.cartService.purchase.purchase.purchasableItems;
    this.dataSource.data = this.purchasableLineItem;
  }

}
