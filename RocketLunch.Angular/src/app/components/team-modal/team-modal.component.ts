import { Component, ViewChild, Input, OnInit } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { User } from '../../models/user';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { Restaurant } from '../../models/restaurant';
import { Team } from 'src/app/models/team';

@Component({
    selector: 'app-team-modal',
    templateUrl: './team-modal.component.html',
    styleUrls: ['./team-modal.component.scss']
})
export class TeamModalComponent implements OnInit {
    @ViewChild(ModalComponent, { static: true }) modal: ModalComponent;
    @Input() user: User;
    @Input() zip: string;
    public teamToAdd: Team;
    constructor(private lunchService: LunchLadyService) { }

    ngOnInit(): void {
        this.teamToAdd = new Team();
        this.teamToAdd.zip = this.zip;
    }

    show() {
        this.modal.show();
    }

    hide() {
        this.modal.hide();
    }

    createTeam() {
        if (this.teamToAdd && this.teamToAdd.name && this.teamToAdd.zip) {
            this.lunchService.createTeam(this.user.id, this.teamToAdd).subscribe(team => { 
                this.user.teams.push(team);
                this.hide();
            });
        }
    }

}