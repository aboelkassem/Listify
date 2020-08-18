import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SongrequestComponent } from './songrequest.component';

describe('SongrequestComponent', () => {
  let component: SongrequestComponent;
  let fixture: ComponentFixture<SongrequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SongrequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SongrequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
