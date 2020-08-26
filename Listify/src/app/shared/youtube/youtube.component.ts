import { YoutubePlayerService } from './youtube.player.service';
import { YoutubeApiService } from './youtube.api.service';
import { Component, Renderer2, AfterContentInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'youtube-component',
  template: '<div id="playerElement"></div>'
})
export class YoutubeComponent implements AfterContentInit {

  // @Input() videoId: string;

  @Output() ready = new EventEmitter<YT.Player>();
  @Output() changed = new EventEmitter<YT.PlayerEvent>();

  ytPlayer: YT.Player;
  changeEvent: YT.PlayerEvent;

  constructor(
    private youtubeApi: YoutubeApiService,
    private youtubePlayer: YoutubePlayerService,
    private renderer: Renderer2
  ) {
    this.youtubeApi.loadApi();
  }

  ngAfterContentInit(): void {
    const elementId = 'playerId';
    const elementContainer = this.renderer.selectRootElement('#playerElement');
    this.renderer.setAttribute(elementContainer, 'id', elementId);

    const config = {
      elementId: elementId,
      width: 300,
      height: 200,
      videoId: '',
      outputs: {
        ready: this.onReady.bind(this),
        change: this.onChange.bind(this)
      }
    };

    this.youtubePlayer.initialise(config);
  }

  onReady(player: YT.Player): void {
    this.ytPlayer = player;
    // this.ytPlayer.loadVideoById(this.videoId);

    this.ready.emit(player);
  }

  onChange(event: YT.PlayerEvent): void {
    this.changed.emit(event);
  }

}
