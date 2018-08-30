import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { Observable, of } from 'rxjs';
import { Restaurant } from '../../models/restaurant';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { HttpService } from '../../services/http.service';
import { AddUserModalComponent } from '../add-user-modal/add-user-modal.component';
import { ModalComponent } from '../modal/modal.component';
import { FormsModule } from '@angular/forms';
import { User } from '../../models/user';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let mockLunchService;

  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['getRestaurant', 'getUsers'])
    mockLunchService.getRestaurant.and.returnValue(of(new Restaurant()));
    mockLunchService.getUsers.and.returnValue(of([new User()]));
    TestBed.configureTestingModule({
      declarations: [HomeComponent, AddUserModalComponent, ModalComponent],
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
  });

  describe('openAddUserModal', () => {
    it('should show modal', () => {
      spyOn(component.addUserModal, 'show').and.returnValue(true);

      component.openAddUserModal();

      expect(component.addUserModal.show).toHaveBeenCalled();
    })
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
});
