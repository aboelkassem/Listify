import { ICurrency } from './../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { HubService } from '../services/hub.service';

@Component({
  selector: 'app-currencies',
  templateUrl: './currencies.component.html',
  styleUrls: ['./currencies.component.css']
})
export class CurrenciesComponent implements OnInit, OnDestroy {

  currencies: ICurrency[] = [];

  currencySubscription: Subscription;

  constructor(private hubService: HubService) {
    this.currencySubscription = this.hubService.getCurrencies().subscribe(currencies => {
      this.currencies = currencies;
    });
  }

  ngOnInit(): void {
    this.hubService.requestCurrencies();
  }
  ngOnDestroy(): void {
    this.currencySubscription.unsubscribe();
  }

  deleteCurrency(id: string): void {
    this.hubService.deleteCurrency(id);
    this.hubService.requestCurrencies();
  }
}
