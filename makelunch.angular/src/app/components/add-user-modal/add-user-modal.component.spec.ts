import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserModalComponent } from './add-user-modal.component';
import { User } from '../../models/user';
import { of } from 'rxjs';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { ModalComponent } from '../modal/modal.component';
import { FormsModule } from '@angular/forms';

describe('AddUserModalComponent', () => {
  let component: AddUserModalComponent;
  let fixture: ComponentFixture<AddUserModalComponent>;
  let mockLunchService: any;

  TestBed.overrideTemplate(
    AddUserModalComponent,
    "<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>");

  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['addUser'])
    mockLunchService.addUser.and.returnValue(of(1));

    TestBed.configureTestingModule({
      declarations: [AddUserModalComponent, ModalComponent],
      providers: [
        { provide: LunchLadyService, useValue: mockLunchService },
      ],
      imports: [FormsModule]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('saveUser', () => {
    it('calls addUser', () => {
      // arrange
      component.newUserName = 'test';

      // act
      component.saveUser();

      // assert
      expect(mockLunchService.addUser).toHaveBeenCalled();
    });

    it('adds user', () => {
      // arrange
      component.users = [new User()];
      component.newUserName = 'test';

      // act
      component.saveUser();

      // assert
      expect(component.users.length).toBe(2);
    });
  });
});
