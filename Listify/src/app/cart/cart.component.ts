import { GlobalsService } from './../services/globals.service';
import { Router } from '@angular/router';
import { IPurchasableItem } from './../interfaces';
import { CartService } from './../services/cart.service';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { IPayPalConfig, ICreateOrderRequest, IPurchaseUnit } from 'ngx-paypal';

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

  payPalConfig?: IPayPalConfig;

  constructor(
    private cartService: CartService,
    private router: Router,
    private globalsService: GlobalsService) {}

  ngOnInit(): void {
    this.dataSource.data = this.purchasableItems;

    if (this.purchasableItems.length > 0) {
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
        size: 'responsive'
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
    this.router.navigate(['/cart']);
  }

  checkOut(): void {

  }
}
