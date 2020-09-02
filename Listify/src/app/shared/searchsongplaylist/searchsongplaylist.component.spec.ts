import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchsongplaylistComponent } from './searchsongplaylist.component';

describe('SearchsongplaylistComponent', () => {
  let component: SearchsongplaylistComponent;
  let fixture: ComponentFixture<SearchsongplaylistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchsongplaylistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchsongplaylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
