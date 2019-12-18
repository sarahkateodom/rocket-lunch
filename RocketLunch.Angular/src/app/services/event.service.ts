import { Team } from 'src/app/models/team';
import { Subject } from 'rxjs';
import { Injectable, EventEmitter } from "@angular/core";
import { User } from '../models/user';

@Injectable({
    providedIn: 'root'
})
export class EventService {
    public selectedTeam = new Subject<Team>();
    teamSelected$ = this.selectedTeam.asObservable();

    public selectedPerson = new Subject<User>();
    personSelected$ = this.selectedPerson.asObservable();

    public loggedInUser = new Subject<User>();
    userLoggedIn$ = this.loggedInUser.asObservable();

    setSelectedTeam(team: Team) {
        console.log('setSelectedTeam', team);
        this.selectedTeam.next(team);
    }

    setSelectedPerson(user: User) {
        this.selectedPerson.next(user);
    }

    userLoggedIn(user: User) {
        console.log('Login Event:', user);
        this.loggedInUser.next(user);
    }
}