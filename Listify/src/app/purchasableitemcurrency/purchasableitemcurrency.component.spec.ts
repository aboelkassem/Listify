import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasableitemcurrencyComponent } from './purchasableitemcurrency.component';

describe('PurchasableitemcurrencyComponent', () => {
  let component: PurchasableitemcurrencyComponent;
  let fixture: ComponentFixture<PurchasableitemcurrencyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchasableitemcurrencyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchasableitemcurrencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
