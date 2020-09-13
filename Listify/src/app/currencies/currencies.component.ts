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

  // deleteCurrency(id: string): void {
  //   this.confirmationModalService.setConfirmationModalMessage('delete this currency');

  //   const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
  //     width: '250px',
  //     data: {isConfirmed: false}
  //   });

  //   confirmationModal.afterClosed().subscribe(result => {
  //     if (result !== undefined) {
  //       this.hubService.deleteCurrency(id);
  //       this.hubService.requestCurrencies();

  //       this.toastrService.success('You have Deleted ' + this.currencies.filter(x => x.id === id)[0].currencyName + ' successfully',
  //         'Deleted Successfully');
  //     }
  //   });
  // }
}
