import { ISongQueued } from './../interfaces';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  currentSong: ISongQueued;

  constructor() { }

  setCurrentSong(currentSong: ISongQueued): void {
    this.currentSong = currentSong;
  }
}
