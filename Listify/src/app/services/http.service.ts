import { observable, Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor() { }

  requestProfileImageUploaded(file: any): Observable<any> {
    // ToDo: need to complete in both backend-frontend
    return null;
  }

  requestRoomImageUploaded(file: any): Observable<any> {
    // ToDo: need to complete in both backend-frontend
    return null;
  }

  requestPlaylistImageUpload(file: any, playlistId: string): Observable<any> {
    // ToDo: need to complete in both backend-frontend
    return null;
  }
}
