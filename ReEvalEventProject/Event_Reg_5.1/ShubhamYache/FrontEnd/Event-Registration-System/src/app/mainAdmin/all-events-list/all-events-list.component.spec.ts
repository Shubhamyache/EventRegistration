import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllEventsListComponent } from './all-events-list.component';

describe('AllEventsListComponent', () => {
  let component: AllEventsListComponent;
  let fixture: ComponentFixture<AllEventsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllEventsListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AllEventsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
