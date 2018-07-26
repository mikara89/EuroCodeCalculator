import { TestBed, inject } from '@angular/core/testing';

import { ArmaturaTypeService } from './armatura-type.service';

describe('ArmaturaTypeService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ArmaturaTypeService]
    });
  });

  it('should be created', inject([ArmaturaTypeService], (service: ArmaturaTypeService) => {
    expect(service).toBeTruthy();
  }));
});
