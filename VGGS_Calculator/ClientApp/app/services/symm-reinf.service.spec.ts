import { TestBed, inject } from '@angular/core/testing';

import { SymmReinfService } from './symm-reinf.service';

describe('SymmReinfService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SymmReinfService]
    });
  });

  it('should be created', inject([SymmReinfService], (service: SymmReinfService) => {
    expect(service).toBeTruthy();
  }));
});
