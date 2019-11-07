import { LunchLadyService } from './../../services/lunch-lady.service';
import { HomeComponent } from './../home/home.component';
import { HttpService } from './../../services/http.service';
import { Subject } from 'rxjs';
import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';

declare const gapi: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit {

  public auth2: any;

  constructor(private service: LunchLadyService, private router: Router) { }

  ngAfterViewInit() {
    this.googleInit();
  }

  public googleInit() {
    gapi.load('auth2', () => {
      this.auth2 = gapi.auth2.init({
        client_id: '330764540748-a2mj7bp7da2mnvk2e9e84ol6c8cbq8dp.apps.googleusercontent.com',
        // cookiepolicy: 'single_host_origin',
        // scope: 'profile email'
      });
      this.attachSignin(document.getElementById('googleBtn'));
    });
  }

  public attachSignin(element) {
    let self = this;
    this.auth2.attachClickHandler(element, {},
      (googleUser) => {
        let profile = googleUser.getBasicProfile();
        console.log('Token || ' + googleUser.getAuthResponse().id_token);
        console.log('ID: ' + profile.getId());
        console.log('Name: ' + profile.getName());
        console.log('Image URL: ' + profile.getImageUrl());
        console.log('Email: ' + profile.getEmail());

        // login user
        this.service.login(profile.getId(), profile.getName(), profile.getEmail())
          .subscribe(x => {
            this.router.navigate(['home']);
          });

      }, (error) => {
        alert(JSON.stringify(error, undefined, 2));
      });
  }

  public signOut() {
    this.auth2.signOut().then(function () {
      console.log('User signed out.');
    });
  }

}
