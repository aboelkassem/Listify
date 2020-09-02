import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchsongrequestComponent } from './searchsongrequest.component';

describe('SearchsongrequestComponent', () => {
  let component: SearchsongrequestComponent;
  let fixture: ComponentFixture<SearchsongrequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchsongrequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchsongrequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
