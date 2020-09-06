import { IPurchasableItem } from './../interfaces';
import { Injectable } from '@angular/core';
import { IPurchaseUnit, ITransactionItem } from 'ngx-paypal';

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
      let discountApplied = 1;
      if (purchasableItem.discountApplied !== undefined || purchasableItem.discountApplied !== null) {
        discountApplied -= purchasableItem.discountApplied;
      }
      total += (purchasableItem.quantity * purchasableItem.unitCost) *  (discountApplied);
    });

    return total;
  }

  updateQuantity(): void {
    this.getSubtotal();
  }

  createPaypalTransaction(): IPurchaseUnit {
    const subTotal = this.getSubtotal();
    const itemList: ITransactionItem[] = [];

    this.purchasableItems.forEach(purchasableItem => {
      const txItem: ITransactionItem = {
        name: purchasableItem.purchasableItemName,
        quantity: purchasableItem.quantity.toString(),
        category: 'DIGITAL_GOODS',
        unit_amount: {
          currency_code: 'USD',
          value: purchasableItem.unitCost.toString(),
        },
        tax: {
          currency_code: 'USD',
          value: '0'
        },
        sku: purchasableItem.id
      };

      itemList.push(txItem);
    });

    const payPalTransaction: IPurchaseUnit = {
      amount: {
        currency_code: 'USD',
        value: subTotal.toString(),
        breakdown: {
          item_total: {
            currency_code: 'USD',
            value: subTotal.toString()
          },
          tax_total: {
            currency_code: 'USD',
            value: '0.00'
          },
          shipping: {
            currency_code: 'USD',
            value: '0.00'
          },
          handling: {
            currency_code: 'USD',
            value: '0'
          },
          insurance: {
            currency_code: 'USD',
            value: '0'
          },
          shipping_discount: {
            currency_code: 'USD',
            value: '0'
          },
        }
      },
      items: itemList
    };

    return payPalTransaction;
  }
}
