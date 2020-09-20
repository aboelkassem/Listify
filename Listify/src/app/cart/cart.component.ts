import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { GlobalsService } from './../services/globals.service';
import { IPurchasableLineItem } from './../interfaces';
import { CartService } from './../services/cart.service';
import { Component, OnInit, OnDestroy, AfterContentInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { OAuthService } from 'angular-oauth2-oidc';
import { HttpClient, HttpHeaders } from '@angular/common/http';

declare let paypal: any;

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit, OnDestroy, AfterContentInit {
  displayedColumns: string[] = ['image', 'itemName', 'orderQuantity', 'cost', 'removeFromCart'];
  dataSource = new MatTableDataSource<IPurchasableLineItem>();

  subTotal: number = this.cartService.getSubtotal();
  purchasableLineItems: IPurchasableLineItem[] = this.cartService.purchasableLineItems;

  paypalLoad = false;
  addScript = false;

  $purchaseSubscription: Subscription;

  constructor(
    private cartService: CartService,
    private globalsService: GlobalsService,
    private router: Router,
    private oauthService: OAuthService,
    private hubService: HubService,
    private http: HttpClient,
    private toastrService: ToastrService) {}

  ngAfterContentInit(): void {
    if (!this.addScript) {
      this.addPaypalScript().then(() => {
        paypal.Buttons({
          createOrder: (data, actions) => {
            const totalPrice = this.cartService.getSubtotal().toString();
            return actions.order.create({
              purchase_units: [{
                amount: {
                  value: totalPrice
                }
              }]
            });
          },
          onApprove: (data, actions) => {
            // This function captures the funds from the transaction.
            // console.log('onApprove - transaction was approved, but not authorized', data, actions);

            actions.order.authorize().then(authorization => {
              this.cartService.createPurchase();
              const payPalCreateRequest = this.cartService.createPaypalTransaction();

              // console.log(data);
              // Get the authorization id
              // const authorizationID = authorization.purchase_units[0].payments.authorizations[0].id;

              // Call your server to validate and capture the transaction
              // return fetch(this.globalsService.developmentWebAPIUrl + 'Paypal/RequestPayment', {
              //   method: 'post',
              //   headers: {
              //     'content-type': 'application/json',
              //     'Authorization': 'Bearer ' + this.oauthService.getAccessToken()
              //   },
              //   body: JSON.stringify({
              //     orderID: data.orderID,
              //     payerID: data.payerID,
              //     order: this.cartService.purchaseOrderRequest,
              //     payment: payPalCreateRequest
              //   })

              const httpHeaders = new HttpHeaders({
                'Content-Type' : 'application/json',
                Authorization: 'Bearer ' + this.oauthService.getAccessToken()
              });

              const options = {
                headers: httpHeaders,
              };

              this.http.post(this.globalsService.developmentWebAPIUrl + 'Paypal/CustomerApproval', {
                  orderID: data.orderID,
                  payerID: data.payerID,
                  order: this.cartService.purchaseOrderRequest,
                  payment: payPalCreateRequest
              }, options).subscribe((links: any) => {
                // console.log(links);
                const approvalLink = links.filter(x => x.rel === 'approval_url')[0];
                window.location.href = approvalLink.href;
              }, error => {
                console.log(error);
              });
            });
          },
          onCancel: (data, actions) => {
            this.toastrService.warning('You have canceled the purchase.', 'Purchase Canceled');
            this.cartService.purchase = undefined;
            this.router.navigateByUrl('/cart');
          },
          onError: err => {
            this.toastrService.error('There are an error with the purchase.', 'Error');
            this.cartService.purchase = undefined;
            this.router.navigateByUrl('/cart');
          },
          onClick: (data, actions) => {
            console.log('onClick', data, actions);
          },
          style: {
            layout:  'vertical',
            color:   'gold',
            shape:   'rect',
            label:   'paypal'
          }
        }).render('#paypal-button-container');

        this.paypalLoad = true;
      });
    }
  }

  ngOnInit(): void {
    this.$purchaseSubscription = this.hubService.getPurchase().subscribe(purchase => {

    });

    this.dataSource.data = this.purchasableLineItems;
  }

  ngOnDestroy(): void {
    this.$purchaseSubscription.unsubscribe();
  }

  // tslint:disable-next-line:typedef
  addPaypalScript(){
    this.addScript = true;
    return new Promise((resolve, reject) => {
      const scripttagElement = document.createElement('script');
      scripttagElement.src = 'https://www.paypal.com/sdk/js?client-id=' + this.globalsService.payPalClientIdSandbox + '&intent=authorize';
      scripttagElement.type = 'text/javascript';
      scripttagElement.onload = resolve;
      document.body.appendChild(scripttagElement);
    });
  }

  updateQuantity(): void {
    this.cartService.updateQuantity();
    this.dataSource.data = this.purchasableLineItems;
    this.subTotal = this.cartService.getSubtotal();
  }

  removeFromCart(id: string): void {
    const selectedItem = this.purchasableLineItems.filter(x => x.purchasableItem.id === id)[0];

    if (selectedItem) {
      this.cartService.removePurchasableLineItemFromCart(id);
      this.dataSource.data = this.purchasableLineItems;
      this.subTotal = this.cartService.getSubtotal();

      this.toastrService.success('You have Removed ' + selectedItem.purchasableItem.purchasableItemName + ' From your cart', 'Removed From Cart');
    }
  }
}
