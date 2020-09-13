import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomkeymodalComponent } from './roomkeymodal.component';

describe('RoomkeymodalComponent', () => {
  let component: RoomkeymodalComponent;
  let fixture: ComponentFixture<RoomkeymodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoomkeymodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoomkeymodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
