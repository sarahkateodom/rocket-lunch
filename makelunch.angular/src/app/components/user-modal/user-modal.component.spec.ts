import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserModalComponent } from './user-modal.component';
import { User } from '../../models/user';
import { of } from 'rxjs';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { ModalComponent } from '../modal/modal.component';
import { FormsModule } from '@angular/forms';
import { Restaurant } from '../../models/restaurant';

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
    it('calls addUser on creation', () => {
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

    it('adds user on creation', () => {
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

    it('calls lunchservice updateUser on edit', () => {
      // arrange
      let user = {
        id: 1,
        name: 'Dorcas',
        nopes: ['Tequila Mockingbird', 'Crapitto\'s'],
      } as User;
      component.user = user;
      component.users = [user];

      // act
      component.saveUser();

      // assert
      expect(mockLunchService.addUser).toHaveBeenCalled();
    });
  });

  describe('nopeRestaurant', () => {
    it('should add restaurant id to user nope list', () => {
      // arrange
      let user = new User();
      user.nopes = [];
      component.users = [user];
      component.user = user;
      const restaurantId = 'test-id';
      component.selectedRestaurantId = restaurantId;

      // act
      component.nopeRestaurant();

      // assert
      expect(component.user.nopes.find(n => n == restaurantId)).toBeTruthy();
    });

    it('set selected restaurant to undefined', () => {
      // arrange
      let user = new User();
      user.nopes = [];
      component.users = [user];
      component.user = user;
      const restaurantId = 'test-id';
      component.selectedRestaurantId = restaurantId;

      // act
      component.nopeRestaurant();

      // assert
      expect(component.selectedRestaurantId).toBeFalsy();
    });
  });

  describe('filterRestaurants', () => {
    it('should filter restaurants to have ones not on nope list', () => {
      // arrange
      let user = new User();
      user.nopes = ['blah1', 'blah2'];
      component.restaurants =[
        {
          id: 'blah1',
        } as Restaurant,
        {
          id: 'blah2',
        } as Restaurant,
        {
          id: 'blah3',
        } as Restaurant
      ];
      component.users = [user];
      component.user = user;

      // act
      let result = component.getFilteredRestaurants();

      // assert
      expect(result.length).toBe(1);
      expect(result[0].id).toBe('blah3');
    });
  });

  describe('getRestaurantNameFromId', () => {
    it('returns name given id', () => {
      // arrange
      component.restaurants =[
        {
          id: 'blah1',
          name: 'name1',
        } as Restaurant,
        {
          id: 'blah2',
          name: 'name2',
        } as Restaurant,
        {
          id: 'blah3',
          name: 'name2',
        } as Restaurant
      ];
      const index = 1;

      // act
      let result = component.getRestaurantNameFromId(component.restaurants[index].id);

      // assert
      expect(result).toBe(component.restaurants[index].name)
    });
  });
});
