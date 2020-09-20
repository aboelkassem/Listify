import { CartService } from './../services/cart.service';
import { IPurchase, IPurchasableItem } from './../interfaces';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {

  displayedColumns: string[] = ['itemName', 'orderQuantity'];
  dataSource = new MatTableDataSource<IPurchasableItem>();

  purchasableLineItem: IPurchasableItem[] = this.cartService.purchase.purchase.purchasableItems;

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.dataSource.data = this.purchasableLineItem;
  }

}
