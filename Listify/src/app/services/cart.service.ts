import { IPurchasableItem } from './../interfaces';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  purchasableItems: IPurchasableItem[] = [];

  constructor() { }

  addPurchasableItemToCart(purchasableItem: IPurchasableItem): void {
    this.purchasableItems.push(purchasableItem);
  }

  removePurchasableItemFromCart(purchasableItem: IPurchasableItem): void {
    const itemInList = this.purchasableItems.filter(x => x.id === purchasableItem.id)[0];

    if (itemInList !== undefined && itemInList !== null) {
      this.purchasableItems.splice(this.purchasableItems.indexOf(itemInList), 1);
    }
  }

  getSubtotal(): number {
    let total = 0;

    this.purchasableItems.forEach(purchasableItem => {
      total += (purchasableItem.quantity * purchasableItem.unitCost);
    });

    return total;
  }

  updateQuantity(): void {
    this.purchasableItems.forEach(purchasableItem => {
      purchasableItem.lineCost = purchasableItem.unitCost * purchasableItem.quantity;
    });

    this.getSubtotal();
  }
}
