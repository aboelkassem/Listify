import { IPurchasableItem, IPurchasableLineItem } from './../interfaces';
import { Injectable } from '@angular/core';
import { IPurchaseUnit, ITransactionItem } from 'ngx-paypal';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  purchasableLineItems: IPurchasableLineItem[] = [];

  constructor() { }

  addPurchasableItemToCart(purchasableItem: IPurchasableItem): void {
    const lineItem: IPurchasableLineItem = {
      id: purchasableItem.id,
      purchasableItemName: purchasableItem.purchasableItemName,
      purchasableItemType: purchasableItem.purchasableItemType,
      quantity: purchasableItem.quantity,
      unitCost: purchasableItem.unitCost,
      imageUri: purchasableItem.imageUri,
      discountApplied: purchasableItem.discountApplied,
      orderQuantity: 1
    };

    this.purchasableLineItems.push(lineItem);
    this.getSubtotal();
  }

  removePurchasableItemFromCart(id: string): void {
    const itemInList = this.purchasableLineItems.filter(x => x.id === id)[0];

    if (itemInList) {
      this.purchasableLineItems.splice(this.purchasableLineItems.indexOf(itemInList), 1);
    }

    this.getSubtotal();
  }

  getSubtotal(): number {
    let total = 0;

    this.purchasableLineItems.forEach(purchasableLineItem => {
      if (purchasableLineItem.orderQuantity <= 0) {
        this.purchasableLineItems.splice(this.purchasableLineItems.indexOf(purchasableLineItem), 1);
      }else {
        let discountApplied = 1;
        if (purchasableLineItem.discountApplied !== undefined || purchasableLineItem.discountApplied !== null) {
          discountApplied -= purchasableLineItem.discountApplied;
        }
        total += purchasableLineItem.orderQuantity * purchasableLineItem.unitCost *  discountApplied;
      }

    });

    return total;
  }

  updateQuantity(): void {
    this.purchasableLineItems.forEach(element => {
      if (element.orderQuantity <= 0) {
        this.purchasableLineItems.splice(this.purchasableLineItems.indexOf(element), 1);
      }
    });

    this.getSubtotal();
  }

  createPaypalTransaction(): IPurchaseUnit {
    const subTotal = this.getSubtotal();
    const itemList: ITransactionItem[] = [];

    this.purchasableLineItems.forEach(purchasableLineItem => {
      const txItem: ITransactionItem = {
        name: purchasableLineItem.purchasableItemName,
        quantity: purchasableLineItem.quantity.toString(),
        category: 'DIGITAL_GOODS',
        unit_amount: {
          currency_code: 'USD',
          value: purchasableLineItem.unitCost.toString(),
        },
        tax: {
          currency_code: 'USD',
          value: '0'
        },
        sku: purchasableLineItem.id
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
