import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyCVComponent } from './apply-cv.component';

describe('ApplyCVComponent', () => {
  let component: ApplyCVComponent;
  let fixture: ComponentFixture<ApplyCVComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplyCVComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplyCVComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
