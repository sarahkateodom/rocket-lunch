import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserModalComponent } from './user-modal.component';
import { User } from '../../models/user';
import { of } from 'rxjs';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { ModalComponent } from '../modal/modal.component';
import { FormsModule } from '@angular/forms';

describe('AddUserModalComponent', () => {
  let component: UserModalComponent;
  let fixture: ComponentFixture<UserModalComponent>;
  let mockLunchService: any;

  TestBed.overrideTemplate(
    UserModalComponent,
    "<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>");

  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['addUser'])
    mockLunchService.addUser.and.returnValue(of(1));

    TestBed.configureTestingModule({
      declarations: [UserModalComponent, ModalComponent],
      providers: [
        { provide: LunchLadyService, useValue: mockLunchService },
      ],
      imports: [FormsModule]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('saveUser', () => {
    it('calls addUser', () => {
      // arrange
      let user = new User();
      component.users = [user];
      component.user = user;
      component.user.name = 'test';
      // act
      component.saveUser();

      // assert
      expect(mockLunchService.addUser).toHaveBeenCalled();
    });

    it('adds user', () => {
      // arrange
      let user = new User();
      component.users = [user];
      component.user = user;
      component.user.name = 'test';

      // act
      component.saveUser();

      // assert
      expect(component.users.length).toBe(2);
    });
  });
});
