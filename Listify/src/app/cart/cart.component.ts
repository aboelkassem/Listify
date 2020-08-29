import { IPurchasableItem } from './../interfaces';
import { CartService } from './../services/cart.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  subTotal: number = this.cartService.getSubtotal();
  purchasableItems: IPurchasableItem[] = this.cartService.purchasableItems;

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
  }

}
