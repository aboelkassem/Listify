import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationusersfollowingmodalComponent } from './applicationusersfollowingmodal.component';

describe('ApplicationusersfollowingmodalComponent', () => {
  let component: ApplicationusersfollowingmodalComponent;
  let fixture: ComponentFixture<ApplicationusersfollowingmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApplicationusersfollowingmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationusersfollowingmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
