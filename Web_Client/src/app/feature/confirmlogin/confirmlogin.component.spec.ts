import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmloginComponent } from './confirmlogin.component';

describe('ConfirmloginComponent', () => {
  let component: ConfirmloginComponent;
  let fixture: ComponentFixture<ConfirmloginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmloginComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
