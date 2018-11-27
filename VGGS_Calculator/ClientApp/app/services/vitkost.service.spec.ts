import { TestBed, inject } from '@angular/core/testing';

import { VitkostService } from './vitkost.service';

describe('VitkostService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [VitkostService]
    });
  });

  it('should be created', inject([VitkostService], (service: VitkostService) => {
    expect(service).toBeTruthy();
  }));
});
