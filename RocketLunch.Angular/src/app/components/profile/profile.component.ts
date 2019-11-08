import { AuthService, SocialUser } from 'angularx-social-login';
import { Restaurant } from './../../models/restaurant';
import { Component, OnInit } from '@angular/core';
import { LunchLadyService } from 'src/app/services/lunch-lady.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  restaurants: Restaurant[];
  selectedRestaurantId: string;
  user: User;
  loading = true;

  constructor(private lunchLady: LunchLadyService) {
    this.getRestaurants();
  }

  ngOnInit() {
    this.lunchLady.getUser(1)
      .subscribe(x => {
        this.user = x;
        console.log(this.user)
      });
  }

  getRestaurants() {
    this.lunchLady.getRestaurants()
      .subscribe(x => {
        this.restaurants = x;
      });
  }

  getFilteredRestaurants() {
    if (!this.user || !this.user.nopes) return this.restaurants;
    return this.restaurants.filter(r => {
      return this.nopesToDisplay().indexOf(r.id) == -1;
    });
  }

  nopesToDisplay(): string[] {
    return this.user.nopes
  }

  getRestaurantNameFromId(id: string): string {
    let restaurant = this.restaurants.find(r => r.id == id);
    return restaurant ? restaurant.name : '';
  }

  removeNope(id: string) {
    this.user.nopes = this.user.nopes.filter(x => x != id);
    this.updateUser();
  }

  nopeRestaurant() {
    if (!this.selectedRestaurantId) return;
    if (!this.user.nopes) this.user.nopes = [];
    
    this.user.nopes.push(this.selectedRestaurantId);
    this.selectedRestaurantId = undefined;
    this.updateUser();
  }

  updateUser() {
    this.lunchLady.updateuser(this.user)
      .subscribe(x => {

      });
  }


}
