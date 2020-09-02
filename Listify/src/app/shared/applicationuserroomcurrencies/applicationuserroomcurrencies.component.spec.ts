import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationuserroomcurrenciesComponent } from './applicationuserroomcurrencies.component';

describe('ApplicationuserroomcurrenciesComponent', () => {
  let component: ApplicationuserroomcurrenciesComponent;
  let fixture: ComponentFixture<ApplicationuserroomcurrenciesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApplicationuserroomcurrenciesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationuserroomcurrenciesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
