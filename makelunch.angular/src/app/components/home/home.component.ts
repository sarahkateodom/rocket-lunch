import { Component, OnInit, ViewChild } from '@angular/core';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { AddUserModalComponent } from '../add-user-modal/add-user-modal.component';
import { User } from '../../models/user';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  @ViewChild(AddUserModalComponent) addUserModal: AddUserModalComponent;
  restaurant: any;
  users: User[] = [];

  constructor(private lunchLady: LunchLadyService) { 
    this.getUsers();
  }

  getRestaurant(): any {
    this.lunchLady.getRestaurant().subscribe(x => {
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
