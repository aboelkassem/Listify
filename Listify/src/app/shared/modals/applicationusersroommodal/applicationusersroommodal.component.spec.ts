import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationusersroommodalComponent } from './applicationusersroommodal.component';

describe('ApplicationusersroommodalComponent', () => {
  let component: ApplicationusersroommodalComponent;
  let fixture: ComponentFixture<ApplicationusersroommodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApplicationusersroommodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationusersroommodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
