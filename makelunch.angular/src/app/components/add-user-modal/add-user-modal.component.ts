import { Component, ViewChild, Input } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { User } from '../../models/user';
import { LunchLadyService } from '../../services/lunch-lady.service';

@Component({
  selector: 'app-add-user-modal',
  templateUrl: './add-user-modal.component.html',
  styleUrls: ['./add-user-modal.component.scss']
})
export class AddUserModalComponent{
  @ViewChild(ModalComponent) modal: ModalComponent;
  @Input() users: User[] = [];
  newUserName: string;

  constructor(private lunchService: LunchLadyService) { }

  show() {
    this.modal.show();
  }

  saveUser() {
    let newUser = { name: this.newUserName } as User;
    this.lunchService.addUser(newUser)
      .subscribe(x => {
        newUser.id = x;
        this.users.push(newUser);
        this.modal.hide();
      });
  }
}
