import { AuthService, SocialUser } from 'angularx-social-login';
import { Restaurant } from './../../models/restaurant';
import { Component, OnInit, ViewChild } from '@angular/core';
import { LunchLadyService } from 'src/app/services/lunch-lady.service';
import { User } from 'src/app/models/user';
import { ActivatedRoute } from '@angular/router';
import { TeamModalComponent } from '../team-modal/team-modal.component';
import { Team } from 'src/app/models/team';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  @ViewChild(TeamModalComponent, { static: true }) teamModal: TeamModalComponent;
  restaurants: Restaurant[];
  selectedRestaurantId: string;
  user: User;
  loading = true;
  subscription;
  team: Team;

  constructor(private lunchLady: LunchLadyService, private activatedRoute: ActivatedRoute) {
    this.team = undefined;
  }

  ngOnInit() {
    this.subscription = this.activatedRoute.params.subscribe(params => {
      let userId = +params['id']; // (+) converts string 'id' to a number
      this.lunchLady.getUser(userId)
        .subscribe(x => {
          this.user = x;
          console.log('Profile user', this.user);
          this.getRestaurants();
        });
    });

  }

  getRestaurants() {
    if (!this.user) return;
    this.lunchLady.getRestaurants(this.user.zip)
      .subscribe(x => {
        this.restaurants = x;
      });
  }

  getFilteredRestaurants() {
    if (!this.user || !this.user.nopes || !this.restaurants) return this.restaurants;
    return this.restaurants.filter(r => {
      return this.nopesToDisplay().indexOf(r.id) == -1;
    });
  }

  nopesToDisplay(): string[] {
    return this.user.nopes
  }

  getRestaurantNameFromId(id: string): string {
    if (!this.restaurants) return '';
    let restaurant = this.restaurants.find(r => r.id == id);
    return restaurant ? restaurant.name : '';
  }

  removeNope(id: string) {
    if (!this.user) return;
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

  openCreateTeamModal() {
    this.team = undefined; // new team
    setTimeout(() => this.teamModal.show(), 33);
  }

  openEditTeamModal(team: Team) {
    this.team = team; // team to edit
    setTimeout(() => this.teamModal.show(), 33);
  }
}
