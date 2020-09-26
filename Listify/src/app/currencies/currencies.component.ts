import { ICurrencyRoom } from './../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { HubService } from '../services/hub.service';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-currencies',
  templateUrl: './currencies.component.html',
  styleUrls: ['./currencies.component.css']
})
export class CurrenciesComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['currencyName'];
  dataSource = new MatTableDataSource<ICurrencyRoom>();

  currencies: ICurrencyRoom[] = [];

  currencySubscription: Subscription;

  constructor(
    private hubService: HubService) {
    this.currencySubscription = this.hubService.getCurrenciesRoom().subscribe(currenciesRoom => {
      this.currencies = currenciesRoom;
      this.dataSource.data = this.currencies;
    });
  }

  ngOnInit(): void {
    this.hubService.requestCurrenciesRoom(this.hubService.applicationUser.room.id);
  }
  ngOnDestroy(): void {
    this.currencySubscription.unsubscribe();
  }
}
