import { FooterComponent } from './../footer/footer.component';
import { NavbarComponent } from './../navbar/navbar.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { Observable, of } from 'rxjs';
import { Restaurant } from '../../models/restaurant';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { HttpService } from '../../services/http.service';
import { ModalComponent } from '../modal/modal.component';
import { FormsModule } from '@angular/forms';
import { User } from '../../models/user';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let mockLunchService;
  let users = [
    { id: 1, name: "Po Labare", nopes: ['fsad', ';lkj'] } as User,
    { id: 2, name: "Pam Dabare", nopes: ['fsad', ';lkj'] } as User,
  ] as User[];

 
  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['getRestaurant', 'getUsers', 'getRestaurants', 'createUserSession', 'updateUserSession', 'updateuser', 'getCurrentUser'])
    mockLunchService.getRestaurant.and.returnValue(of(new Restaurant()));
    mockLunchService.getRestaurant.and.returnValue(of(new Restaurant()));
    mockLunchService.getRestaurants.and.returnValue(of([new Restaurant()]));
    mockLunchService.getCurrentUser.and.returnValue(of(users[0]));
    mockLunchService.getUsers.and.returnValue(of(users));
    mockLunchService.createUserSession.and.returnValue(of("12324124-123123-123123"));
    mockLunchService.updateUserSession.and.returnValue(of(true));
    mockLunchService.updateuser.and.returnValue(of(true));
      
    TestBed.configureTestingModule({
      declarations: [HomeComponent, ModalComponent],
      providers: [
        { provide: LunchLadyService, useValue: mockLunchService },
        { provide: HttpService, useClass: HttpService }
      ],
      imports: [FormsModule]
    })
    .overrideTemplate(
      HomeComponent,
      "<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>")
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

  describe('ngOnInit', () => {
    it('creates user session', () => {
      component = fixture.componentInstance;
      component.ngOnInit();
      expect(mockLunchService.createUserSession).toHaveBeenCalledWith([]);
    })

    it('calls getRestaurant', () => {
      component = fixture.componentInstance;
      component.ngOnInit();
      expect(mockLunchService.getRestaurant).toHaveBeenCalled();
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
});
