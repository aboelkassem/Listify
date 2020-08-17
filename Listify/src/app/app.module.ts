import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { OAuthModule } from 'angular-oauth2-oidc';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { YoutubeModule } from 'angularx-youtube';

import { AppComponent } from './app.component';
import { YoutubeComponent } from './youtube/youtube.component';
import { HomeComponent } from './home/home.component';
import { SearchComponent } from './shared/search/search.component';
import { ChatComponent } from './chat/chat.component';
import { AccountComponent } from './account/account.component';

@NgModule({
  declarations: [
    AppComponent,
    YoutubeComponent,
    HomeComponent,
    SearchComponent,
    ChatComponent,
    AccountComponent
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
