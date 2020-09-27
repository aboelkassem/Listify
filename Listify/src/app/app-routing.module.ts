import { ProfileComponent } from './profile/profile.component';
import { PlaylistscommunityComponent } from './playlistscommunity/playlistscommunity.component';
import { PurchasesComponent } from './purchases/purchases.component';
import { CheckoutfailComponent } from './checkoutfail/checkoutfail.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { PurchasableitemsComponent } from './purchasableitems/purchasableitems.component';
import { CurrencyComponent } from './currency/currency.component';
import { CurrenciesComponent } from './currencies/currencies.component';
import { HomeComponent } from './home/home.component';
import { RoomsComponent } from './rooms/rooms.component';
import { AccountComponent } from './account/account.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoomComponent } from './room/room.component';
import { PlaylistsComponent } from './playlists/playlists.component';
import { PlaylistComponent } from './playlist/playlist.component';
import { CartComponent } from './cart/cart.component';

const routes: Routes = [
  // {path: '', redirectTo: 'home', pathMatch: 'full'},
  {path: '', component: HomeComponent},
  {path: 'rooms', component: RoomsComponent},
  {path: 'playlist', component: PlaylistComponent},
  {path: 'playlist/:id', component: PlaylistComponent},
  {path: 'playlists', component: PlaylistsComponent},
  {path: 'playlistsCommunity', component: PlaylistscommunityComponent},
  {path: 'currency', component: CurrencyComponent},
  {path: 'currency/:id', component: CurrencyComponent},
  {path: 'currencies', component: CurrenciesComponent},
  {path: 'purchases', component: PurchasesComponent},
  {path: 'purchasableItems', component: PurchasableitemsComponent},
  {path: 'account', component: AccountComponent},
  {path: 'cart', component: CartComponent},
  {path: 'checkout', component: CheckoutComponent},
  {path: 'checkoutfail', component: CheckoutfailComponent},
  {path: 'profile/:id', component: ProfileComponent},
  // {path: ':id', component: HomeComponent},
  {path: ':id', component: RoomComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
