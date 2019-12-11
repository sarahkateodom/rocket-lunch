import { Component, OnInit, ViewChild } from '@angular/core';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { User } from '../../models/user';
import { UUID } from 'angular2-uuid';
import { Restaurant } from '../../models/restaurant';
import { MealTime } from '../../models/enums/MealTime';
import { RestaurantSearch } from 'src/app/models/restaurant-search';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  restaurant: any;
  goSrc: string;
  goSrcs: string[];
  users: User[] = [];
  selectedUser: User;
  restaurants: Restaurant[] = [];
  sessionId: UUID;
  sliderSrc: string = './assets/sun.png';
  meal: MealTime;
  isLunch: boolean = false;
  isDinner: boolean = false;

  constructor(private lunchLady: LunchLadyService) {

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

    // this.setBreakfast();
  }

  setRandomGoImage() {
    let randomIndex = Math.floor(Math.random() * this.goSrcs.length);
    this.goSrc = this.goSrcs[randomIndex];
  }

  getRestaurant(): any {

    if (!this.sessionId) this.sessionId = UUID.UUID();
    this.lunchLady.getRestaurant(this.sessionId, new RestaurantSearch()).subscribe(x => {
      this.restaurant = x;
    });
  }

  // toggleMeal() {
  //   if (this.meal == MealTime.breakfast) {
  //     this.setLunch();
  //   }
  //   else if (this.meal == MealTime.lunch) {
  //     this.setDinner();
  //   }
  //   else if (this.meal == MealTime.dinner) {
  //     this.setBreakfast();
  //   }
  // }

  // setBreakfast() {
  //   this.meal = MealTime.breakfast;
  //   this.isLunch = false;
  //   this.isDinner = false;
  //   this.sliderSrc = './assets/morning.png';
  // }

  // setLunch() {
  //   this.meal = MealTime.lunch;
  //   this.isLunch = true;
  //   this.isDinner = false;
  //   this.sliderSrc = './assets/day.png';
  // }

  // setDinner() {
  //   this.meal = MealTime.dinner;
  //   this.isLunch = false;
  //   this.isDinner = true;
  //   this.sliderSrc = './assets/night.png';
  // }

}
