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
  nopesToBeRemovedOnSave: string[] = [];
  nopesToBeAddedOnSave: string[] = [];
  selectedRestaurantId: string;

  constructor(private lunchService: LunchLadyService) { }

  show() {
    this.modal.show();
  }

  hide() {
    this.modal.hide();
    this.nopesToBeRemovedOnSave = [];
    this.nopesToBeAddedOnSave = [];
    
  }

  nopeRestaurant() {
    if (!this.user.nopes) this.user.nopes = [];
    if(this.selectedRestaurantId) this.user.nopes.push(this.selectedRestaurantId);
    this.selectedRestaurantId = undefined;
  }

  getFilteredRestaurants() {
    if (!this.user.nopes) return this.restaurants;

    return this.restaurants.filter(r => {
      return this.nopesToDisplay().indexOf(r.id) == -1;
    });
  }

  saveUser() {
    this.user.nopes = this.nopesToDisplay();
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

  nopesToDisplay(): string[] {
    return this.user.nopes.concat(this.nopesToBeAddedOnSave).filter(n => !this.nopesToBeRemovedOnSave.find(x => x == n));
  }

  getRestaurantNameFromId(id: string): string {
    let restaurant = this.restaurants.find(r => r.id == id);
    return restaurant ? restaurant.name : '';
  }

  removeNope(id: string) {
    // this.user.nopes = this.user.nopes.filter(x => x != id);
    this.nopesToBeRemovedOnSave.push(id);
  }
}
