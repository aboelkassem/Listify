import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileplaylistsComponent } from './profileplaylists.component';

describe('ProfileplaylistsComponent', () => {
  let component: ProfileplaylistsComponent;
  let fixture: ComponentFixture<ProfileplaylistsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfileplaylistsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileplaylistsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
