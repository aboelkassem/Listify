import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckoutfailComponent } from './checkoutfail.component';

describe('CheckoutfailComponent', () => {
  let component: CheckoutfailComponent;
  let fixture: ComponentFixture<CheckoutfailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckoutfailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckoutfailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
