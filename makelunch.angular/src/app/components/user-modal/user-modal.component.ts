import { Component, ViewChild, Input, OnInit } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { User } from '../../models/user';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { Restaurant } from '../../models/restaurant';

@Component({
  selector: 'app-user-modal',
  templateUrl: './user-modal.component.html',
  styleUrls: ['./user-modal.component.scss']
})
export class UserModalComponent {
  @ViewChild(ModalComponent) modal: ModalComponent;
  @Input() user: User;
  @Input() users: User[] = [];
  @Input() restaurants: Restaurant[] = [];
  selectedRestaurantId: string;

  constructor(private lunchService: LunchLadyService) { }

  show() {
    this.modal.show();
  }

  hide() {
    this.modal.hide();
  }

  nopeRestaurant() {
    if(this.selectedRestaurantId) this.user.nopes.push(this.selectedRestaurantId);
    this.selectedRestaurantId = undefined;
  }

  getFilteredRestaurants() {
    return this.restaurants.filter(r => {
      return this.user.nopes.indexOf(r.id) == -1;
    });
  }

  saveUser() {
    if (this.user.id) {
      // edit user
      this.lunchService.updateuser(this.user)
        .subscribe(x => {
          this.hide();
        });

      this.hide();
    } else {
      // add user
      this.lunchService.addUser(this.user)
        .subscribe(x => {
          this.user.id = x;
          this.users.push(this.user);
          this.hide();
        });
    }
  }

  getRestaurantNameFromId(id: string): string {
    return this.restaurants.find(r => r.id == id) ? this.restaurants.find(r => r.id == id).name : "";
  }
}
