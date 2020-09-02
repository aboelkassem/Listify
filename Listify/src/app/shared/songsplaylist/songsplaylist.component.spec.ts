import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SongsplaylistComponent } from './songsplaylist.component';

describe('SongsplaylistComponent', () => {
  let component: SongsplaylistComponent;
  let fixture: ComponentFixture<SongsplaylistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SongsplaylistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SongsplaylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
