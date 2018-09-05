import { Component, OnInit, ViewChild } from '@angular/core';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { UserModalComponent } from '../user-modal/user-modal.component';
import { User } from '../../models/user';
import { UUID } from 'angular2-uuid';
import { Restaurant } from '../../models/restaurant';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  @ViewChild(UserModalComponent) userModal: UserModalComponent;
  restaurant: any;
  goSrc: string;
  goSrcs: string[];
  users: User[] = [];
  selectedUser: User;
  restaurants: Restaurant[] = [];
  sessionId: UUID;

  constructor(private lunchLady: LunchLadyService) {
    this.getUsers();
    this.getRestaurants();

    this.goSrcs = [
      './assets/go-burger.png',
      './assets/go-chicken-leg.png',
      './assets/go-donut.png',
      './assets/go-parfait.png',
      './assets/go-pizza.png',
      './assets/go-salad.png',
      './assets/go-sandwich.png',
      './assets/go-taco.png',
      './assets/go-tea.png',
      './assets/go-coffee-break.png',
      './assets/go-cookies.png',
      './assets/go-pint.png',
      './assets/go-pancake.png',
    ];

    this.setRandomGoImage();
    window.setInterval(() => {
      this.setRandomGoImage();
    }, 2000);
  }

  setRandomGoImage() {
    let randomIndex = Math.floor(Math.random() * this.goSrcs.length);
    this.goSrc = this.goSrcs[randomIndex];
  }

  getRestaurant(): any {
    
    if (!this.sessionId) this.sessionId = UUID.UUID();
    this.lunchLady.getRestaurant(this.sessionId).subscribe(x => {
      this.restaurant = x;
    });
  }

  openAddUserModal() {
    this.selectedUser = new User();
    this.userModal.show();
  }

  getUsers() {
    this.lunchLady.getUsers()
      .subscribe(x => {
        this.users = x;
        this.lunchLady.createUserSession(x.map(u => u.id))
        .subscribe(y => {
          
          this.sessionId = y;
        });
      });
  }

  getRestaurants() {
    this.lunchLady.getRestaurants()
      .subscribe(x => {
        this.restaurants = x;
      });
  }

  selectUser(user: User) {
    this.selectedUser = user;
    this.userModal.show();
  }

  dismissUser(userId: number) {
    this.users = this.users.filter(u => u.id != userId);
    this.lunchLady.updateUserSession(this.sessionId, this.users.map(u => u.id))
      .subscribe(x => {});
  }

}
