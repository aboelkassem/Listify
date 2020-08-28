import { ICurrency } from './../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { HubService } from '../services/hub.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-currency',
  templateUrl: './currency.component.html',
  styleUrls: ['./currency.component.css']
})
export class CurrencyComponent implements OnInit, OnDestroy {

  id: string;
  currencyName: string;
  weight: number;
  quantityIncreasePerTick: number;
  timeSecBetweenTick: number;
  roomId: string; // not used yet

  currencySubscription: Subscription;

  constructor(
    private hubService: HubService,
    private router: Router,
    private route: ActivatedRoute) {

    this.currencySubscription = this.hubService.getCurrency().subscribe(currency => {
      this.id = currency.id;
      this.currencyName = currency.currencyName;
      this.weight = currency.weight;
      this.quantityIncreasePerTick = currency.quantityIncreasePerTick;
      this.timeSecBetweenTick = currency.timeSecBetweenTick;
    });

    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id != null) {
        this.getCurrency(id);
      }
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.currencySubscription.unsubscribe();
  }

  saveCurrency(): void {
    const request: ICurrency = {
      id: this.id,
      currencyName: this.currencyName,
      weight: this.weight,
      quantityIncreasePerTick: this.quantityIncreasePerTick,
      timeSecBetweenTick: this.timeSecBetweenTick
    };

    this.hubService.saveCurrency(request);

    this.router.navigateByUrl('/currencies');
  }

  getCurrency(id: string): void {
    this.hubService.requestCurrency(id);
  }

}
