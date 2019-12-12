import { LunchLadyService } from './../../services/lunch-lady.service';
import { AuthService, GoogleLoginProvider, SocialUser } from 'angularx-social-login';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  public loading: boolean = true;
  public internalUser: User;

  constructor(private service: LunchLadyService, private authService: AuthService) { }

  ngOnInit() {
    this.service.getCurrentUser()
      .subscribe(i => {
        this.internalUser = i;
        this.loading = false;
      });
  }

  signIn(): void {
    let signInPromise = this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
    signInPromise.then(socialUser => {
      this.service.login(socialUser)
        .subscribe(u => {
          this.internalUser = u;
          console.log('Internal User', u);
        });
    });
  }

  signOut(): void {
    let signOutPromise = this.authService.signOut();
    signOutPromise.then(x => {
      this.service.logout()
        .subscribe(success => {
          if (success) this.internalUser = undefined;
        });
    });
  }
}
