import { Component, OnInit } from '@angular/core';
import { LunchLadyService } from '../../services/lunch-lady.service';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  restaurant: any;

  constructor(private lunchLady: LunchLadyService) { }

  getRestaurant(): any {
    this.lunchLady.getRestaurant().subscribe(x => {
      this.restaurant = x;
    });
  }

}
