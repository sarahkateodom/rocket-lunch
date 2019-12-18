import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ModalComponent } from '../components/modal/modal.component';
import { User } from '../models/user';
import { LunchLadyService } from '../services/lunch-lady.service';
import { EventService } from '../services/event.service';
import { Team } from '../models/team';

@Component({
  selector: 'app-explorers-modal',
  templateUrl: './explorers-modal.component.html',
  styleUrls: ['./explorers-modal.component.scss']
})
export class ExplorersModalComponent implements OnInit {

  constructor(private service: LunchLadyService, private eventService: EventService) { }

  @ViewChild(ModalComponent, { static: true }) modal: ModalComponent;
  @Input() user: User;
  // selectedEmail: string;
  selectedTeamId: any;

  ngOnInit() {
  }

  show() {
    this.modal.show();
  }

  hide() {
    this.modal.hide();
  }

  addExplorers() {
    let team = undefined;
    if(this.selectedTeamId) {
      team = this.user.teams.find(t => t.id == this.selectedTeamId);
    } 
    this.eventService.setSelectedTeam(team);
    this.hide();
  }

}
