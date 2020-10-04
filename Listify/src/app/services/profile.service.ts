import { Subject, Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  applicationUserId: string;

  $applicationUserIdChanged = new Subject<string>();

  constructor() { }

  setApplicationUserId(id: string): void {
    this.applicationUserId = id;

    this.$applicationUserIdChanged.next(this.applicationUserId);
  }

  getApplicationUserId(): Observable<string> {
    return this.$applicationUserIdChanged.asObservable();
  }
}
