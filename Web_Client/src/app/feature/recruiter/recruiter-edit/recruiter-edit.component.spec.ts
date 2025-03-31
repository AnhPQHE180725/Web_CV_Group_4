import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterEditComponent } from './recruiter-edit.component';

describe('RecruiterEditComponent', () => {
  let component: RecruiterEditComponent;
  let fixture: ComponentFixture<RecruiterEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
