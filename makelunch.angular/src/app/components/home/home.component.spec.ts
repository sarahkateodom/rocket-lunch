import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { Observable, of } from 'rxjs';
import { Restaurant } from '../../models/restaurant';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { HttpService } from '../../services/http.service';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let mockLunchService;

  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['getRestaurant'])
    mockLunchService.getRestaurant.and.returnValue(of(new Restaurant()));
    TestBed.configureTestingModule({
      declarations: [HomeComponent],
      providers: [
        { provide: LunchLadyService, useValue: mockLunchService },
        { provide: HttpService, useClass: HttpService }
      ]
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
});
