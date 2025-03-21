import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterHomepageComponent } from './recruiter-homepage.component';

describe('RecruiterHomepageComponent', () => {
  let component: RecruiterHomepageComponent;
  let fixture: ComponentFixture<RecruiterHomepageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterHomepageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterHomepageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
