import { HubService } from 'src/app/services/hub.service';
import { GlobalsService } from './../services/globals.service';
import { Router } from '@angular/router';
import { IPurchasableLineItem, IPurchaseCreateRequest } from './../interfaces';
import { CartService } from './../services/cart.service';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { IPayPalConfig, ICreateOrderRequest, IPurchaseUnit } from 'ngx-paypal';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  displayedColumns: string[] = ['itemName', 'orderQuantity', 'cost', 'removeFromCart'];
  dataSource = new MatTableDataSource<IPurchasableLineItem>();

  subTotal: number = this.cartService.getSubtotal();
  purchasableLineItems: IPurchasableLineItem[] = this.cartService.purchasableLineItems;

  payPalConfig?: IPayPalConfig;

  constructor(
    private cartService: CartService,
    private globalsService: GlobalsService,
    private hubService: HubService,
    private toastrService: ToastrService) {}

  ngOnInit(): void {
    this.dataSource.data = this.purchasableLineItems;

    if (this.purchasableLineItems.length > 0) {
      this.initPayPalConfig();
    }
  }

  private initPayPalConfig(): void {
    this.payPalConfig = {
      currency: 'EUR',
      clientId: this.globalsService.payPalClientIdSandbox,
      advanced: {
        commit: 'true'
      },
      style: {
        label: 'paypal',
        layout: 'vertical',
        shape: 'rect',
        size: 'large'
      },
      onApprove: (data, actions) => {
        console.log('onApprove - transaction was approved, but not authorized', data, actions);
        actions.order.get().then(details => {
          console.log('onApprove - you can get full order details inside onApprove: ', details);
        });
      },
      onClientAuthorization: (data) => {
        console.log('onClientAuthorization - you should probably inform your server about completed transaction at this point', data);
        // this.showSuccess = true;
      },
      onCancel: (data, actions) => {
        console.log('OnCancel', data, actions);
      },
      onError: err => {
        console.log('OnError', err);
      },
      onClick: (data, actions) => {
        console.log('onClick', data, actions);
      },
      // tslint:disable-next-line:no-angle-bracket-type-assertion
      createOrderOnClient: (data) => <ICreateOrderRequest> {
        intent: 'CAPTURE',
        purchase_units: [
          this.cartService.createPaypalTransaction()
        ]
      },
    };
  }

  updateQuantity(): void {
    this.cartService.updateQuantity();
    this.dataSource.data = this.purchasableLineItems;
    this.subTotal = this.cartService.getSubtotal();
  }

  removeFromCart(id: string): void {
    this.cartService.removePurchasableItemFromCart(id);
    this.dataSource.data = this.purchasableLineItems;
    this.subTotal = this.cartService.getSubtotal();

    const selectedItem = this.purchasableLineItems.filter(x => x.purchasableItem.id === id)[0];
    this.toastrService.success('You have Removed ' + selectedItem.purchasableItem.purchasableItemName + ' From your cart', 'Removed From Cart');
  }

  checkOut(): void {

  }

  createPurchase(): void {
    const json: string[] = [];

    this.cartService.purchasableLineItems.forEach(element => {
      json.push(JSON.stringify(element));
    });

    const purchase: IPurchaseCreateRequest = {
      id: '',
      purchaseMethod: this.globalsService.getPurchaseMethodType('Paypal'),
      subtotal: this.cartService.getSubtotal(),
      amountCharged: this.cartService.getSubtotal(),
      purchasableItemsJSON: json
    };

    this.hubService.createPurchase(purchase);
  }
}
