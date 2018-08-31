import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TooManyRequestsComponent } from './too-many-requests.component';

describe('TooManyRequestsComponent', () => {
  let component: TooManyRequestsComponent;
  let fixture: ComponentFixture<TooManyRequestsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TooManyRequestsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TooManyRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
