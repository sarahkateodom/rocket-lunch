import { AuthService } from 'angularx-social-login';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarComponent } from './navbar.component';
import { LunchLadyService } from 'src/app/services/lunch-lady.service';
import { of } from 'rxjs';
import { User } from 'src/app/models/user';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  let mockLunchService: any;
  let mockAuthService: any;
  beforeEach(async(() => {
    mockLunchService = jasmine.createSpyObj('LunchLadyService', ['login', 'logout']);
    mockAuthService = jasmine.createSpyObj('AuthService', ['authState']);
    mockAuthService.authState.and.returnValue(of({ id: 1, name: 'test' } as User));
    TestBed.configureTestingModule({
      declarations: [ NavbarComponent ],
      providers: [
        { provide: LunchLadyService, useValue: mockLunchService },
        { provide: AuthService, useValue: mockAuthService },
      ],
    })
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
