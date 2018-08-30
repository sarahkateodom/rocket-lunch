import { Component, ViewChild, Input, OnInit } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { User } from '../../models/user';
import { LunchLadyService } from '../../services/lunch-lady.service';

@Component({
  selector: 'app-user-modal',
  templateUrl: './user-modal.component.html',
  styleUrls: ['./user-modal.component.scss']
})
export class UserModalComponent {
  @ViewChild(ModalComponent) modal: ModalComponent;
  @Input() user: User;
  @Input() users: User[] = [];

  constructor(private lunchService: LunchLadyService) { }

  show() {
    if (!this.user) {
      this.user = new User();
    }

    this.modal.show();
  }

  hide() {
    this.user = undefined;
    this.modal.hide();
  }

  saveUser() {
    if (this.user.id) {
      // edit user

      this.hide();
    } else {
      // add user
      this.lunchService.addUser(this.user)
        .subscribe(x => {
          this.user.id = x;
          this.users.push(this.user);
          this.hide();
        });
    }
  }
}
