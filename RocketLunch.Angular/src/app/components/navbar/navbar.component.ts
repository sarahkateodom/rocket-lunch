import { LunchLadyService } from './../../services/lunch-lady.service';
import { AuthService, GoogleLoginProvider } from 'angularx-social-login';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  public user: any;

  constructor(private service: LunchLadyService, private authService: AuthService) { }

  ngOnInit() {
    this.authService.authState.subscribe((user) => {
      this.user = user;
    });
  }

  signIn(): void {
    let signInPromise = this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
    signInPromise.then(x => {
      this.service.login(x.id, x.name, x.email)
        .subscribe(() => { });
    });
  }

  signOut(): void {
    let signOutPromise = this.authService.signOut();
    signOutPromise.then(x => {
      this.service.logout()
        .subscribe(y => { });
    });
  }
}
