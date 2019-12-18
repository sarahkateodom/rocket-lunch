import { EventService } from './../../services/event.service';
import { LunchLadyService } from './../../services/lunch-lady.service';
import { AuthService, GoogleLoginProvider, SocialUser } from 'angularx-social-login';
import { Component, OnInit, Output, EventEmitter, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { User } from 'src/app/models/user';
import { Team } from 'src/app/models/team';
import { Router } from '@angular/router';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  public loading: boolean = true;
  public internalUser: User;
  
  constructor(private service: LunchLadyService, private eventService: EventService, private authService: AuthService, private router: Router) { 
  }

  ngOnInit() {
    this.service.getCurrentUser()
      .subscribe(i => {
        console.log('Current user', i)
        this.internalUser = i;
        this.loading = false;
      }, err => {
        this.loading = false;
      });  
  }

  selectTeam(team: Team) {
    this.eventService.setSelectedTeam(team);
  }

  signIn(): void {
    let self = this;
    let signInPromise = this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
    signInPromise.then(socialUser => {
      // login
      self.service.login(socialUser)
        .subscribe(u => {
          self.internalUser = u;
          console.log('SignIn User', u);
          if (!self.internalUser.zip) {
            // get lng/lat from browser
            self.service.getPosition()
              .then((x) => {
                // get geocode location
                self.service.getGeocodeResult(x.lng, x.lat)
                  .subscribe(geocode => {
                    let zip = geocode.features.filter(feature => feature.place_type.find(place => place == "postcode"))[0].text;

                    // update user with zip
                    self.internalUser.zip = zip;
                    self.service.updateuser(self.internalUser)
                      .subscribe(updateResult => { });
                  });
              });
          }
        });
    });
  }


  signOut(): void {
    let signOutPromise = this.authService.signOut();
    signOutPromise.then(x => {
      this.service.logout()
        .subscribe(success => {
          if (success) this.internalUser = undefined;
          this.router.navigate(['/']);
        });
    });
  }

}
