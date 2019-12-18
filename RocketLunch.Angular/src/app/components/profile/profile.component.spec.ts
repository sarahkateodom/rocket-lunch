import { FooterComponent } from './../footer/footer.component';
import { NavbarComponent } from './../navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileComponent } from './profile.component';
import { of } from 'rxjs';
import { LunchLadyService } from 'src/app/services/lunch-lady.service';
import { User } from 'src/app/models/user';
import { Restaurant } from 'src/app/models/restaurant';
import { ActivatedRoute } from '@angular/router';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let mockLunchService: any;

  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['updateuser', 'getRestaurants', 'getUser'])
    mockLunchService.getUser.and.returnValue(of({ id: 1, name: 'test' } as User));
    mockLunchService.getRestaurants.and.returnValue(of([]));
    mockLunchService.updateuser.and.returnValue(of(true));

    TestBed.configureTestingModule({
      declarations: [ProfileComponent],
      providers: [
        { provide: ActivatedRoute, useValue: { params: of({ id: 123 }) } },
        { provide: LunchLadyService, useValue: mockLunchService },
      ],
      imports: [FormsModule]
    })
      .overrideTemplate(
        ProfileComponent,
        "<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>")
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('updateUser', () => {

    it('calls lunchservice updateUser on edit', () => {
      // arrange
      let user = {
        id: 1,
        name: 'Dorcas',
        nopes: ['Tequila Mockingbird', 'Crapitto\'s'],
      } as User;
      component.user = user;

      // act
      component.updateUser();

      // assert
      expect(mockLunchService.updateuser).toHaveBeenCalled();

    });
  });

  describe('getRestaurantNameFromId', () => {
    it('returns name given id', () => {
      // arrange
      component.restaurants = [
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
    it('removes nope from user nopes', () => {
      // arrange
      component.user = new User();
      component.user.nopes = ['1', '2'];

      // act
      component.removeNope('1');

      // assert
      expect(component.user.nopes.find(n => n == '1')).toBeFalsy();
    });

    it('calls updateUser', () => {
      // arrange
      component.user = new User();
      component.user.nopes = ['1', '2'];

      // act
      component.removeNope('1');

      // assert
      expect(mockLunchService.updateuser).toHaveBeenCalledTimes(1);
    });
  });

  describe('toggleEditZip', () => {
    it('toggles from false to true', () => {
      // arrange
      component.editingZip = false;

      // act
      component.toggleEditZip();

      // asert
      expect(component.editingZip).toBeTruthy();
    });

    it('toggles from true to false', () => {
      // arrange
      component.editingZip = true;

      // act
      component.toggleEditZip();

      // asert
      expect(component.editingZip).toBeFalsy();
    });
  });
});
