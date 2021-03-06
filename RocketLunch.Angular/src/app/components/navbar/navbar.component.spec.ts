import { AuthService } from 'angularx-social-login';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarComponent } from './navbar.component';
import { LunchLadyService } from 'src/app/services/lunch-lady.service';
import { of } from 'rxjs';
import { User } from 'src/app/models/user';
import { Router } from '@angular/router';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  let mockLunchService: any;
  let mockAuthService: any;
  let mockRouter: any;

  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['login', 'logout', 'getCurrentUser']);
    mockLunchService.getCurrentUser.and.returnValue(of(new User()));
    mockAuthService = jasmine.createSpyObj('AuthService', ['authState']);
    mockAuthService.authState = (of({ id: 1, name: 'test' } as User));

    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      declarations: [NavbarComponent],
      providers: [
        { provide: LunchLadyService, useValue: mockLunchService },
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
      ],
    })
    .overrideTemplate(
      NavbarComponent,
      "<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>")
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
