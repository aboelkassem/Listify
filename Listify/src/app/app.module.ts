import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { OAuthModule } from 'angular-oauth2-oidc';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { YoutubeModule } from 'angularx-youtube';

import { AppComponent } from './app.component';
import { SearchComponent } from './shared/search/search.component';
import { ChatComponent } from './chat/chat.component';
import { AccountComponent } from './account/account.component';
import { RoomsComponent } from './rooms/rooms.component';
import { RoomComponent } from './room/room.component';
import { HomeComponent } from './home/home.component';
import { QueueComponent } from './queue/queue.component';
import { CurrencyComponent } from './currency/currency.component';
import { CurrenciesComponent } from './currencies/currencies.component';

@NgModule({
  declarations: [
    AppComponent,
    SearchComponent,
    ChatComponent,
    AccountComponent,
    RoomsComponent,
    RoomComponent,
    HomeComponent,
    QueueComponent,
    CurrencyComponent,
    CurrenciesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    OAuthModule.forRoot(),
    YoutubeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
