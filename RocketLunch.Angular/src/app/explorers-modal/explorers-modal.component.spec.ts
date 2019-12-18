import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExplorersModalComponent } from './explorers-modal.component';

describe('ExplorersModalComponent', () => {
  let component: ExplorersModalComponent;
  let fixture: ComponentFixture<ExplorersModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExplorersModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExplorersModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
