import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmregisterComponent } from './confirmregister.component';

describe('ConfirmregisterComponent', () => {
  let component: ConfirmregisterComponent;
  let fixture: ComponentFixture<ConfirmregisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmregisterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmregisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
