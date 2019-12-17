import { Team } from 'src/app/models/team';
import { Subject } from 'rxjs';
import { Injectable, EventEmitter } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class EventService {
    public selectedTeam = new Subject<Team>();
    teamSelected$ = this.selectedTeam.asObservable();

    setSelectedTeam(team: Team) {
        console.log('setSelectedTeam', team);
        this.selectedTeam.next(team);
    }
}