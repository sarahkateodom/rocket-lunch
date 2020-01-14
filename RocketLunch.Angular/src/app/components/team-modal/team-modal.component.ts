import { Component, ViewChild, Input, OnInit, ElementRef, NgZone } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { User } from '../../models/user';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { Restaurant } from '../../models/restaurant';
import { Team } from 'src/app/models/team';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject, fromEvent } from 'rxjs';
import { EventService } from 'src/app/services/event.service';

@Component({
    selector: 'app-team-modal',
    templateUrl: './team-modal.component.html',
    styleUrls: ['./team-modal.component.scss']
})
export class TeamModalComponent {
    @ViewChild(ModalComponent, { static: true }) modal: ModalComponent;
    @ViewChild('nameField', { static: false }) nameField: ElementRef;
    @ViewChild('zipField', { static: false }) zipField: ElementRef;
    @Input() user: User;
    @Input() existingTeam: Team;
    public selectedTeam: Team;
    public users: User[] = [];
    public userToAddEmail: string = '';
    public errorMessage: string;

    constructor(private lunchService: LunchLadyService, private eventService: EventService) { }

    show() {
        this.errorMessage = '';
        if (!this.existingTeam) {
            this.selectedTeam = {
                zip: this.user ? this.user.zip : ''
            } as Team;
        } else {
            this.selectedTeam = Object.assign(new Team(), this.existingTeam) as Team;
            this.lunchService.getTeamUsers(this.selectedTeam.id)
                .subscribe(users => this.users = users);
        }

        this.modal.show();
        this.setUpInputs();
    }

    hide() {
        this.modal.hide();
    }

    setUpInputs() {
        setTimeout(() => {
            fromEvent(this.nameField.nativeElement, "keyup").pipe(
                debounceTime(1000),
            ).subscribe(res => {
                this.updateTeam();
            });

            fromEvent(this.zipField.nativeElement, "keyup").pipe(
                debounceTime(1000),
            ).subscribe(res => {
                this.updateTeam();
            });
        }, 200);

    }

    filterUsers() {
        return this.users.filter(u => u.id != this.user.id);
    }

    createTeam() {
        this.errorMessage = '';
        if (this.user && this.selectedTeam && this.selectedTeam.name && this.selectedTeam.zip) {
            this.lunchService.createTeam(this.user.id, this.selectedTeam)
                .subscribe(team => {
                    this.user.teams.push(team);
                    this.hide();
                }, err => {
                    this.errorMessage = err.error;
                });
        }
    }

    updateTeam() {
        if (this.selectedTeam.id && this.selectedTeam.name && this.selectedTeam.zip.length == 5) {
            this.errorMessage = '';

            // update team
            this.lunchService.updateTeam(this.selectedTeam)
                .subscribe(x => {
                    this.eventService.updateUserTeam(this.selectedTeam);
                }, err => {
                    this.errorMessage = err.error;
                });
        }
    }

    addUser() {
        this.errorMessage = '';
        if (this.selectedTeam && this.userToAddEmail) {
            this.lunchService.addUserToTeam(this.userToAddEmail, this.selectedTeam.id)
                .subscribe(user => {
                    this.users.push(user);
                    this.userToAddEmail = '';
                }, err => {
                    this.errorMessage = err.error;
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