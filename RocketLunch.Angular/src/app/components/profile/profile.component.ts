import { NopesModalComponent } from './../nopes-modal/nopes-modal.component';
import { AuthService, SocialUser } from 'angularx-social-login';
import { Restaurant } from './../../models/restaurant';
import { Component, OnInit, ViewChild } from '@angular/core';
import { LunchLadyService } from 'src/app/services/lunch-lady.service';
import { User } from 'src/app/models/user';
import { ActivatedRoute, Router } from '@angular/router';
import { TeamModalComponent } from '../team-modal/team-modal.component';
import { Team } from 'src/app/models/team';
import { EventService } from 'src/app/services/event.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  @ViewChild(TeamModalComponent, { static: true }) teamModal: TeamModalComponent;
  @ViewChild(NopesModalComponent, { static: true }) nopesModal: NopesModalComponent;
  restaurants: Restaurant[];
  selectedRestaurantId: string;
  user: User;
  userId: number;
  loading = true;
  subscription;
  team: Team;
  editingZip: boolean = false;

  constructor(private lunchLady: LunchLadyService, private eventService: EventService, private activatedRoute: ActivatedRoute, private router: Router) {
    this.team = undefined;
  }

  ngOnInit() {
    this.subscription = this.activatedRoute.params.subscribe(params => {
      this.userId = +params['id']; // (+) converts string 'id' to a number
      let getUserResponse = this.getUser();
      getUserResponse.subscribe(x => this.getRestaurants());
    });

    this.eventService.updatedUserTeam$.subscribe(updatedTeam => {
      debugger;
      let teamIndex = this.user.teams.findIndex(x => x.id == updatedTeam.id);
      this.user.teams[teamIndex] = Object.assign(new Team(), updatedTeam);
    });

  }

  getUser() {
    let getUserResponse = this.lunchLady.getUser(this.userId);
    getUserResponse
      .subscribe(x => {
        this.user = x;
        if (!this.user.nopes) this.user.nopes = [];
      }, err => {
        this.router.navigate(['/'])
      });

    return getUserResponse;
  }

  getRestaurants() {
    if (!this.user) return;
    this.lunchLady.getRestaurants(this.user.zip)
      .subscribe(x => {
        this.restaurants = x;
      });
  }

  nopesToDisplay(): string[] {
    return this.user.nopes.filter(x => this.getRestaurantNameFromId(x));
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

  updateUser() {
    this.lunchLady.updateuser(this.user)
      .subscribe(x => {

      });
  }

  leaveTeam(team: Team, index: number, e: any) {
    e.stopPropagation();
    this.lunchLady.removeUserFromTeam(this.user.id, team.id)
      .subscribe(res => this.user.teams.splice(index, 1));
  }

  openNopesModal() {
    setTimeout(() => this.nopesModal.show(), 33);
  }

  openCreateTeamModal() {
    this.team = undefined; // new team
    setTimeout(() => this.teamModal.show(), 33);
  }

  openEditTeamModal(team: Team) {
    this.team = team; // team to edit
    setTimeout(() => this.teamModal.show(), 33);
  }

  toggleEditZip() {
    this.editingZip = !this.editingZip;
  }
}
