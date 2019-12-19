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
    @Input() team: Team;
    public selectedTeam: Team;
    public users: User[] = [];
    public userToAddEmail: string = '';
    constructor(private lunchService: LunchLadyService) { }

    ngOnInit(): void {
        // if team is set to existing team, edit this.team
        // otherwise, a new team will be made
    }

    show() {
        if (!this.team) {
            this.selectedTeam = new Team();
            this.selectedTeam.zip = this.user ? this.user.zip : '';
        } else {
            this.selectedTeam = Object.assign(new Team(), this.team);
            this.lunchService.getTeamUsers(this.selectedTeam.id)
                .subscribe(users => this.users = users);
        }

        this.modal.show();
    }

    hide() {
        this.modal.hide();
    }

    filteredUsers() {
        return this.users.filter(u => u.id != this.user.id);
    }

    createTeam() {
        if (this.selectedTeam && this.selectedTeam.name && this.selectedTeam.zip) {
            this.lunchService.createTeam(this.user.id, this.selectedTeam).subscribe(team => {
                this.user.teams.push(team);
                this.hide();
            });
        }
    }

    addUser() {
        if (this.selectedTeam && this.userToAddEmail) {
            this.lunchService.addUserToTeam(this.userToAddEmail, this.selectedTeam.id)
                .subscribe(user => {
                    this.users.push(user);
                    this.userToAddEmail = '';
                });
        }
    }

    removeUserFromTeam(userId: number, index: number) {
        this.lunchService.removeUserFromTeam(userId, this.selectedTeam.id)
            .subscribe(res => {
                this.users.splice(index, 1);
            });
    }

}