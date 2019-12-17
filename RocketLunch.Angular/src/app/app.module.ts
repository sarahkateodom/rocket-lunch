import { TeamModalComponent } from './components/team-modal/team-modal.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LunchLadyService } from './services/lunch-lady.service';
import { HttpService } from './services/http.service';
import { HttpClientModule } from '@angular/common/http';
import { APP_BASE_HREF } from '@angular/common';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { ModalComponent } from './components/modal/modal.component';
import { TooManyRequestsComponent } from './components/too-many-requests/too-many-requests.component';
import { routing } from './app.routes';
import { SocialLoginModule, AuthServiceConfig } from "angularx-social-login";
import { GoogleLoginProvider, FacebookLoginProvider } from "angularx-social-login";
import { ProfileComponent } from './components/profile/profile.component';

 
let config = new AuthServiceConfig([
  {
    id: GoogleLoginProvider.PROVIDER_ID,
    provider: new GoogleLoginProvider("330764540748-a2mj7bp7da2mnvk2e9e84ol6c8cbq8dp.apps.googleusercontent.com")
  },
]);
 
export function provideConfig() {
  return config;
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    FooterComponent,
    ModalComponent,
    TooManyRequestsComponent,
    ProfileComponent,
    TeamModalComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    routing,
    SocialLoginModule,
  ],
  providers: [
    HttpService,
    LunchLadyService, 
    { provide: APP_BASE_HREF, useValue: '/' },
    { provide: AuthServiceConfig, useFactory: provideConfig },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
