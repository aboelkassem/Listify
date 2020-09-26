import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistscommunityComponent } from './playlistscommunity.component';

describe('PlaylistscommunityComponent', () => {
  let component: PlaylistscommunityComponent;
  let fixture: ComponentFixture<PlaylistscommunityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlaylistscommunityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlaylistscommunityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
