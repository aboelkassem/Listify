import { GlobalsService } from './../services/globals.service';
import { HubService } from './../services/hub.service';
import { RoomHubService } from './../services/room-hub.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  roomCode: string;

  constructor(
    private route: ActivatedRoute,
    private hubService: HubService,
    private globalsService: GlobalsService,
    private roomService: RoomHubService) {
    this.route.params.subscribe(params => {
      this.roomCode = params['id'];
      if (this.roomCode) {
        this.roomService.requestRoom(this.roomCode);
      }else {
        // disconnect from your room and
        // get your default room
        if (this.hubService.isConnected && this.hubService.applicationUser && !this.roomService.applicationUserRoom.isOwner) {
          this.roomService.disconnectFromHub();
          this.roomService.connectToHub(this.globalsService.developmentWebAPIUrl + 'roomHub', undefined);
        }
      }
    });
   }

  ngOnInit(): void {
  }

}
