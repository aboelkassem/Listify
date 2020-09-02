import { Router } from '@angular/router';
import { IPurchasableItem } from './../interfaces';
import { CartService } from './../services/cart.service';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  displayedColumns: string[] = ['itemName', 'quantity', 'cost'];
  dataSource = new MatTableDataSource<IPurchasableItem>();

  subTotal: number = this.cartService.getSubtotal();
  purchasableItems: IPurchasableItem[] = this.cartService.purchasableItems;

  constructor(
    private cartService: CartService,
    private router: Router) { }

  ngOnInit(): void {
    this.dataSource.data = this.purchasableItems;
  }

  updateQuantity(): void {
    this.cartService.updateQuantity();
    this.router.navigate(['/cart']);
  }
}
