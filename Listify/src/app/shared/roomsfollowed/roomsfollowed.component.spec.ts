import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomsfollowedComponent } from './roomsfollowed.component';

describe('RoomsfollowedComponent', () => {
  let component: RoomsfollowedComponent;
  let fixture: ComponentFixture<RoomsfollowedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoomsfollowedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoomsfollowedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
