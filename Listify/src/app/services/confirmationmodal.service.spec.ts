import { TestBed } from '@angular/core/testing';

import { ConfirmationmodalService } from './confirmationmodal.service';

describe('ConfirmationmodalService', () => {
  let service: ConfirmationmodalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConfirmationmodalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
