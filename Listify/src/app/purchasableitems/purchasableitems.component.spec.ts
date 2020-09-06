import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasableitemsComponent } from './purchasableitems.component';

describe('PurchasableitemsComponent', () => {
  let component: PurchasableitemsComponent;
  let fixture: ComponentFixture<PurchasableitemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchasableitemsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchasableitemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
