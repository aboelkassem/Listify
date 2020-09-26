import { MaterialModule } from './material.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { OAuthModule } from 'angular-oauth2-oidc';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { ColorPickerModule } from 'ngx-color-picker';
import { NgxLoadingXModule } from 'ngx-loading-x';
import { YoutubeModule } from 'angularx-youtube';
// import { YoutubeModule } from './youtube';
// import { YoutubeModule } from './shared/youtube';
// import { NgxYoutubePlayerModule  } from "ngx-youtube-player";
// import { YouTubePlayerModule } from '@angular/youtube-player';

import { AppComponent } from './app.component';
import { ChatComponent } from './shared/chat/chat.component';
import { AccountComponent } from './account/account.component';
import { RoomsComponent } from './rooms/rooms.component';
import { RoomComponent } from './room/room.component';
import { HomeComponent } from './home/home.component';
import { QueueComponent } from './shared/queue/queue.component';
import { CurrencyComponent } from './currency/currency.component';
import { CurrenciesComponent } from './currencies/currencies.component';
import { PlaylistComponent } from './playlist/playlist.component';
import { PlaylistsComponent } from './playlists/playlists.component';
import { PlayerComponent } from './shared/player/player.component';
import { CartComponent } from './cart/cart.component';
import { ApplicationuserroomcurrenciesComponent } from './shared/applicationuserroomcurrencies/applicationuserroomcurrencies.component';
import { SearchsongplaylistComponent } from './shared/searchsongplaylist/searchsongplaylist.component';
import { SongsplaylistComponent } from './shared/songsplaylist/songsplaylist.component';
import { ConfirmationmodalComponent } from './shared/modals/confirmationmodal/confirmationmodal.component';
import { SearchsongrequestComponent } from './shared/searchsongrequest/searchsongrequest.component';
import { PurchasableitemsComponent } from './purchasableitems/purchasableitems.component';
import { InputmodalComponent } from './shared/modals/inputmodal/inputmodal.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { CheckoutfailComponent } from './checkoutfail/checkoutfail.component';
import { PurchasesComponent } from './purchases/purchases.component';
import { PlaylistscommunityComponent } from './playlistscommunity/playlistscommunity.component';
import { ApplicationusersroommodalComponent } from './shared/modals/applicationusersroommodal/applicationusersroommodal.component';
import { InformationmodalComponent } from './shared/modals/informationmodal/informationmodal.component';

@NgModule({
  declarations: [
    AppComponent,
    ChatComponent,
    AccountComponent,
    RoomsComponent,
    RoomComponent,
    HomeComponent,
    QueueComponent,
    CurrencyComponent,
    CurrenciesComponent,
    PlaylistComponent,
    PlaylistsComponent,
    PlayerComponent,
    CartComponent,
    ApplicationuserroomcurrenciesComponent,
    SearchsongplaylistComponent,
    SearchsongrequestComponent,
    SongsplaylistComponent,
    ConfirmationmodalComponent,
    PurchasableitemsComponent,
    InputmodalComponent,
    CheckoutComponent,
    CheckoutfailComponent,
    PurchasesComponent,
    PlaylistscommunityComponent,
    ApplicationusersroommodalComponent,
    InformationmodalComponent,
  ],
  entryComponents: [
    ConfirmationmodalComponent,
    InputmodalComponent,
    ApplicationusersroommodalComponent,
    InformationmodalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    OAuthModule.forRoot(),
    YoutubeModule,
    BrowserAnimationsModule,
    ColorPickerModule,
    MaterialModule,
    NgxLoadingXModule,
    ToastrModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
