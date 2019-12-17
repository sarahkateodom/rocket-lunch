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
export class HomeComponent implements OnInit {
  restaurant: any;
  goSrc: string;
  goSrcs: string[];
  internalUser: User;
  users: User[] = [];
  selectedUser: User;
  restaurants: Restaurant[] = [];
  sessionId: UUID;
  sliderSrc: string = './assets/sun.png';
  meal: MealTime;
  isLunch: boolean = false;
  isDinner: boolean = false;
  zip: string;

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

  ngOnInit() {
    this.lunchLady.getCurrentUser()
      .subscribe(i => {
        this.internalUser = i;
        this.zip = this.internalUser.zip;
      }, err => {
        this.lunchLady.getPosition()
          .then((x) => {
            // get geocode location
            this.lunchLady.getGeocodeResult(x.lng, x.lat)
              .subscribe(geocode => {
                this.zip = geocode.features.filter(feature => feature.place_type.find(place => place == "postcode"))[0].text;
              });
          });
      });
  }


  setRandomGoImage() {
    let randomIndex = Math.floor(Math.random() * this.goSrcs.length);
    this.goSrc = this.goSrcs[randomIndex];
  }

  getRestaurant(): any {
    let searchOptions = new RestaurantSearch();
    searchOptions.zip = this.zip;
    if (!this.sessionId) {
      this.sessionId = UUID.UUID();
    }

    this.lunchLady.getRestaurant(this.sessionId, searchOptions)
      .subscribe(x => {
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
