import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterCandidateListComponent } from './recruiter-candidate-list.component';

describe('RecruiterCandidateListComponent', () => {
  let component: RecruiterCandidateListComponent;
  let fixture: ComponentFixture<RecruiterCandidateListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterCandidateListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterCandidateListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
