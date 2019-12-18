import { Component, ViewChild, Input, OnInit } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { User } from '../../models/user';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { Restaurant } from '../../models/restaurant';

@Component({
  selector: 'app-nopes-modal',
  templateUrl: './nopes-modal.component.html',
  styleUrls: ['./nopes-modal.component.scss']
})
export class NopesModalComponent {
  @ViewChild(ModalComponent, { static: true }) modal: ModalComponent;
  @Input() user: User;
  public restaurants: Restaurant[] = [];
  nopesToBeRemovedOnSave: string[] = [];
  selectedRestaurantId: string;

  constructor(private lunchService: LunchLadyService) { }

  show() {
    this.getRestaurants();
    this.modal.show();
  }

  hide() {
    this.modal.hide();
    this.nopesToBeRemovedOnSave = [];
  }

  getRestaurants() {
    if (!this.user) return;
    this.lunchService.getRestaurants(this.user.zip)
      .subscribe(x => {
        this.restaurants = x;
      });
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
          this.selectedRestaurantId = undefined;
        });
    }
  }

  nopesToDisplay(): string[] {
    return this.user.nopes.filter(n => !this.nopesToBeRemovedOnSave.find(x => x == n));
  }

  getRestaurantNameFromId(id: string): string {
    let restaurant = this.restaurants.find(r => r.id == id);
    return restaurant ? restaurant.name : '';
  }

  nopeRestaurant() {
    if (!this.user.nopes) this.user.nopes = [];
    if (this.selectedRestaurantId) this.user.nopes.push(this.selectedRestaurantId);
    this.saveUser();
  }

  removeNope(id: string) {
    this.nopesToBeRemovedOnSave.push(id);
    this.saveUser();
  }
}