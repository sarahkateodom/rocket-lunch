import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NopesModalComponent } from './nopes-modal.component';
import { User } from '../../models/user';
import { of } from 'rxjs';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { ModalComponent } from '../modal/modal.component';
import { FormsModule } from '@angular/forms';
import { Restaurant } from '../../models/restaurant';

describe('NopesModalComponent', () => {
  let component: NopesModalComponent;
  let fixture: ComponentFixture<NopesModalComponent>;
  let mockLunchService: any;

  TestBed.overrideTemplate(
    NopesModalComponent,
    "<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>");

  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['addUser', 'updateuser'])
    mockLunchService.addUser.and.returnValue(of(1));
    mockLunchService.updateuser.and.returnValue(of(true));

    TestBed.configureTestingModule({
      declarations: [NopesModalComponent, ModalComponent],
      providers: [
        { provide: LunchLadyService, useValue: mockLunchService },
      ],
      imports: [FormsModule]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NopesModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('saveUser', () => {
    
    it('calls lunchservice updateUser on edit', () => {
      // arrange
      let user = {
        id: 1,
        name: 'Dorcas',
        nopes: ['Tequila Mockingbird', 'Crapitto\'s'],
      } as User;
      component.user = user;

      // act
      component.saveUser();

      // assert
      expect(mockLunchService.updateuser).toHaveBeenCalled();

    });
  });

  describe('nopeRestaurant', () => {
    it('should add restaurant id to user nopes', () => {
      // arrange
      let user = new User();
      user.nopes = [];
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

  describe('removeNope', () => {
    it('removes nope from nopesToBeRemovedOnSave', () => {
      // arrange
      component.user = new User();
      component.user.nopes = ['1', '2'];

      // act
      component.removeNope('1');

      // assert
      expect(component.nopesToBeRemovedOnSave.length).toBe(1);
      expect(component.nopesToBeRemovedOnSave).toContain('1');
    });
  })
});