import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { Observable, of } from 'rxjs';
import { Restaurant } from '../../models/restaurant';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { HttpService } from '../../services/http.service';
import { UserModalComponent } from '../user-modal/user-modal.component';
import { ModalComponent } from '../modal/modal.component';
import { FormsModule } from '@angular/forms';
import { User } from '../../models/user';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let mockLunchService;

  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['getRestaurant', 'getUsers', 'getRestaurants'])
    mockLunchService.getRestaurant.and.returnValue(of(new Restaurant()));
    mockLunchService.getRestaurants.and.returnValue(of([new Restaurant()]));
    mockLunchService.getUsers.and.returnValue(of([new User()]));
    TestBed.configureTestingModule({
      declarations: [HomeComponent, UserModalComponent, ModalComponent],
      providers: [
        { provide: LunchLadyService, useValue: mockLunchService },
        { provide: HttpService, useClass: HttpService }
      ], 
      imports: [FormsModule]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ctor', () => {
    it('gets users', () => {
      component = fixture.componentInstance;
      expect(mockLunchService.getUsers).toHaveBeenCalled();
    })

    it('gets restaurants', () => {
      component = fixture.componentInstance;
      expect(mockLunchService.getRestaurants).toHaveBeenCalled();
    })
  });

  describe('getRestaurant', () => {
    it('calls getRestaurant from service', () => {
      // arrange
      component.restaurant = undefined;

      // act
      component.getRestaurant();

      // assert
      expect(mockLunchService.getRestaurant).toHaveBeenCalled();
      expect(component.restaurant).toBeTruthy();
    });
    it('calls getRestaurant from service and makes new guid', () => {
      // arrange
      component.sessionId = undefined;
      component.restaurant = undefined;

      // act
      component.getRestaurant();

      // assert
      expect(component.sessionId).not.toBe(undefined);
    });
  });

  describe('openAddUserModal', () => {
    it('should show modal', () => {
      spyOn(component.userModal, 'show').and.returnValue(true);

      component.openAddUserModal();

      expect(component.userModal.show).toHaveBeenCalled();
    });

    it('should set selectedUser new user', () => {
      // arrange
      component.selectedUser = {
        id: 1,
        name: 'SHmmeve',
        nopes: ['123'],
      } as User;

      // act
      component.openAddUserModal();

      // assert
      expect(component.selectedUser.id).toBe(undefined);
      expect(component.selectedUser.name).toBe('');
      expect(component.selectedUser.nopes.length).toBe(0);
    });
  });

  describe('getUsers', () => {
    it('calls getUsers from service', () => {
      // arrange
      component.users = [];

      // act
      component.getUsers();

      // assert
      expect(mockLunchService.getUsers).toHaveBeenCalled();
    });
    it('adds users to component', () => {
      // arrange
      component.users = [];

      // act
      component.getUsers();

      // assert
      expect(component.users.length).toBe(1);
    });
  });

  describe('selectUser', () => {
    it('sets selectedUser', () => {
      // arrange
      component.selectedUser = undefined;
      let user = new User();
      user.id = 1;

      // act
      component.selectUser(user);

      // assert
      expect(component.selectedUser).toBeTruthy();
      expect(component.selectedUser.id).toBe(user.id);
    });

    it('opens users modal', () => {
      // arrange
      spyOn(component.userModal, 'show').and.returnValue(true);

      // act
      component.selectUser(new User());

      // assert
      expect(component.userModal.show).toHaveBeenCalled();
    });
  });

  describe('dismissUser', () => {
    it('should remove specified user from component.users', () => {
      // arrange
      let user = new User();
      user.id = 1;

      component.users = [user, { id: 2 } as User, { id: 3 } as User];
      let length = component.users.length;

      // act
      component.dismissUser(user.id);

      // assert
      expect(length - 1).toBe(component.users.length);
    });
  });
});
