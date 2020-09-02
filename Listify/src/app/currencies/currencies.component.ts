import { ICurrency } from './../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { HubService } from '../services/hub.service';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmationmodalService } from '../services/confirmationmodal.service';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationmodalComponent } from '../shared/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-currencies',
  templateUrl: './currencies.component.html',
  styleUrls: ['./currencies.component.css']
})
export class CurrenciesComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['currencyName', 'deleteCurrency'];
  dataSource = new MatTableDataSource<ICurrency>();

  currencies: ICurrency[] = [];

  currencySubscription: Subscription;

  constructor(
    private hubService: HubService,
    private toastrService: ToastrService,
    private confirmationModal: MatDialog,
    private confirmationModalService: ConfirmationmodalService) {
    this.currencySubscription = this.hubService.getCurrencies().subscribe(currencies => {
      this.currencies = currencies;
      this.dataSource.data = this.currencies;
    });
  }

  ngOnInit(): void {
    this.hubService.requestCurrencies();
  }
  ngOnDestroy(): void {
    this.currencySubscription.unsubscribe();
  }

  deleteCurrency(id: string): void {
    this.confirmationModalService.setConfirmationModalMessage('delete this currency');

    const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
      width: '250px',
      data: {isConfirmed: false}
    });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.hubService.deleteCurrency(id);
        this.hubService.requestCurrencies();

        this.toastrService.success('You have Deleted ' + this.currencies.filter(x => x.id === id)[0].currencyName + ' successfully',
          'Deleted Successfully');
      }
    });
  }
}
