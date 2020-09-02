import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationmodalService {

  confirmationModalMessage: string;

  constructor() { }

  setConfirmationModalMessage(message: string): void {
    this.confirmationModalMessage = message;
  }
}
