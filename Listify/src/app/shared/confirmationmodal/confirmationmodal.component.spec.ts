import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogmodalComponent } from './dialogmodal.component';

describe('DialogmodalComponent', () => {
  let component: DialogmodalComponent;
  let fixture: ComponentFixture<DialogmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
