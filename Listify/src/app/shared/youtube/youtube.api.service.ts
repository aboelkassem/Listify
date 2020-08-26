import { Injectable, EventEmitter } from '@angular/core';
import { get } from 'http';

const getWindow = () => window;

@Injectable()
export class YoutubeApiService {

  public apiEmitter: EventEmitter<any> = new EventEmitter<any>();

  private _window: any;
  private hasLoaded = false; // may want to control this better - eg if api is unloaded and needs to be reinitialised

  constructor() {
    this._window = getWindow();
  }

  loadApi(): void {
    if (!this.hasLoaded) {
      const scriptTag = this._window.document.createElement('script');
      scriptTag.type = 'text/javascript';
      scriptTag.src = 'https://www.youtube.com/iframe_api';
      this._window.document.body.appendChild(scriptTag);

      this._window['onYoutubeIframeAPIReady'] = () => {
        this.apiEmitter.emit(this._window['YT']);
      };

      this.hasLoaded = true;
    }
  }
}
