import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewParticipationComponent } from './view-participation.component';

describe('ViewParticipationComponent', () => {
  let component: ViewParticipationComponent;
  let fixture: ComponentFixture<ViewParticipationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewParticipationComponent]
    });
    fixture = TestBed.createComponent(ViewParticipationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
