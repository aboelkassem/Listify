import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InformationmodalComponent } from './informationmodal.component';

describe('InformationmodalComponent', () => {
  let component: InformationmodalComponent;
  let fixture: ComponentFixture<InformationmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InformationmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InformationmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
