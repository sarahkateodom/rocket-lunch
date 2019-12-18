import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { User } from '../../models/user';
import { UUID } from 'angular2-uuid';
import { Restaurant } from '../../models/restaurant';
import { MealTime } from '../../models/enums/MealTime';
import { RestaurantSearch } from 'src/app/models/restaurant-search';
import { EventService } from 'src/app/services/event.service';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
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
  loading: boolean = false;

  constructor(private lunchLady: LunchLadyService, private eventService: EventService) {
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

    this.eventService.teamSelected$.subscribe(selectedTeam => {
      this.lunchLady.getTeamUsers(selectedTeam.id)
        .subscribe(users => this.users = users);
    });
  }

  ngOnInit() {
    this.lunchLady.createUserSession([])
      .subscribe(sessionId => {
        this.sessionId = sessionId;

        this.lunchLady.getCurrentUser()
          .subscribe(i => {
            this.internalUser = i;
            this.zip = this.internalUser.zip;
            this.getRestaurant();
          }, err => {
            this.lunchLady.getPosition()
              .then((x) => {
                // get geocode location
                this.lunchLady.getGeocodeResult(x.lng, x.lat)
                  .subscribe(geocode => {
                    this.zip = geocode.features.filter(feature => feature.place_type.find(place => place == "postcode"))[0].text;
                    this.getRestaurant();
                  });
              });
          });
      });
  }

  setRandomGoImage() {
    let randomIndex = Math.floor(Math.random() * this.goSrcs.length);
    this.goSrc = this.goSrcs[randomIndex];
  }

  getRestaurant(): any {
    this.loading = true;
    let searchOptions = new RestaurantSearch();
    searchOptions.zip = this.zip;
    searchOptions.userIds = !this.users ? [this.internalUser.id] : this.users.map(u => u.id);

    if (!this.sessionId) {
      this.sessionId = UUID.UUID();
    }

    this.lunchLady.getRestaurant(this.sessionId, searchOptions)
      .subscribe(x => {
        this.restaurant = x;
        this.loading = false;
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
