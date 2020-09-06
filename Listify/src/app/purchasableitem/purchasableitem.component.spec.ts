import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasableitemComponent } from './purchasableitem.component';

describe('PurchasableitemComponent', () => {
  let component: PurchasableitemComponent;
  let fixture: ComponentFixture<PurchasableitemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchasableitemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchasableitemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
