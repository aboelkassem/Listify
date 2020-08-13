import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor() { }

  search(query: string): Subject<string> {
    return new Subject<string>();
  }
}
