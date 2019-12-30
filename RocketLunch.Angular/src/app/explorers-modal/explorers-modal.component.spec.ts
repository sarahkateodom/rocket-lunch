import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExplorersModalComponent } from './explorers-modal.component';
import { ModalComponent } from '../components/modal/modal.component';
import { LunchLadyService } from '../services/lunch-lady.service';
import { FormsModule } from '@angular/forms';
import { EventService } from '../services/event.service';

describe('ExplorersModalComponent', () => {
  let component: ExplorersModalComponent;
  let fixture: ComponentFixture<ExplorersModalComponent>;
  let mockEventService;

  beforeEach(async(() => {
    mockEventService = jasmine.createSpyObj('EventService', ['setSelectedTeam'])

    TestBed.configureTestingModule({
      declarations: [ ExplorersModalComponent, ModalComponent ],
      providers: [
        { provide: EventService, useValue: mockEventService },
      ],
      imports: [FormsModule]
    })
    .overrideTemplate(
      ExplorersModalComponent,
      "<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>")
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
