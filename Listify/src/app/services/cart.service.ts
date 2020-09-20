import { GlobalsService } from './globals.service';
import { IPurchasableLineItem, IPurchaseConfirmed, IPurchaseOrderRequest } from './../interfaces';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  purchasableLineItems: IPurchasableLineItem[] = [];
  purchase: IPurchaseConfirmed;
  purchaseOrderRequest: IPurchaseOrderRequest;

  constructor(private globalsService: GlobalsService) { }

  addPurchasableItemToCart(purchasableLineItem: IPurchasableLineItem): void {
    this.purchasableLineItems.push(purchasableLineItem);
    this.getSubtotal();
  }

  removePurchasableLineItemFromCart(id: string): void {
    const itemInList = this.purchasableLineItems.filter(x => x.purchasableItem.id === id)[0];

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
        if (purchasableLineItem.purchasableItem.discountApplied !== undefined ||
          purchasableLineItem.purchasableItem.discountApplied !== null) {
          discountApplied -= purchasableLineItem.purchasableItem.discountApplied;
        }
        total += purchasableLineItem.orderQuantity * purchasableLineItem.purchasableItem.unitCost *  discountApplied;
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

  createPaypalTransaction(): any {
    const itemList: any[] = [];

    this.purchasableLineItems.forEach(purchasableLineItem => {
      const txItem: any = {
        name: purchasableLineItem.purchasableItem.purchasableItemName,
        quantity: purchasableLineItem.orderQuantity.toString(),
        price: purchasableLineItem.purchasableItem.unitCost.toString(),
        tax: '0',
        sku: purchasableLineItem.purchasableItem.id,
        currency: 'USD'
      };

      itemList.push(txItem);
    });

    const paypalPaymentCreateRequest: any = {
      intent: 'sale',
      payer: {
        payment_method: 'paypal'
      },
      transactions: [
        {
          amount: {
            currency: 'USD',
            total: this.getSubtotal().toString(),
            details: {
              subtotal: this.getSubtotal().toString(),
              tax: '0',
              shipping: '0',
              handling_fee: '0',
              shipping_discount: '0',
              insurance: '0',
            }
          },
          item_list: {
            items : itemList
          },
        }
      ],
      note_to_payer: 'Contact us for any questions on your order.',
      redirect_urls: {
        return_url: 'http://localhost:4200/home',
        cancel_url: 'http://localhost:4200/cart'
      }
    };

    return paypalPaymentCreateRequest;
  }

  clearCart(): void {
    this.purchasableLineItems = [];
  }

  createPurchase(): void {
    const json: string[] = [];

    this.purchasableLineItems.forEach(element => {
      json.push(JSON.stringify(element));
    });

    const purchaseOrderRequest: IPurchaseOrderRequest = {
      purchaseMethod: this.globalsService.getPurchaseMethodType('Paypal'),
      subtotal: this.getSubtotal(),
      amountCharged: this.getSubtotal(),
      purchasableItemsJSON: json
    };

    this.purchaseOrderRequest = purchaseOrderRequest;
  }

  createPurchaseConfirmed(purchase: IPurchaseConfirmed): void {
    this.clearCart();
    this.purchase = purchase;
  }
}
