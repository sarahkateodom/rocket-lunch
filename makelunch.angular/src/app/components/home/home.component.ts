import { Component, OnInit, ViewChild } from '@angular/core';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { AddUserModalComponent } from '../add-user-modal/add-user-modal.component';
import { User } from '../../models/user';
import { UUID } from 'angular2-uuid';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  @ViewChild(AddUserModalComponent) addUserModal: AddUserModalComponent;
  restaurant: any;
  goSrc: string;
  goSrcs: string[];
  users: User[] = [];
  sessionId: UUID = undefined;

  constructor(private lunchLady: LunchLadyService) {
    this.getUsers();

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
    this.addUserModal.show();
  }

  getUsers() {
    this.lunchLady.getUsers()
      .subscribe(x => {
        this.users = x;
      });
  }

}
