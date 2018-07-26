import { TestBed, inject } from '@angular/core/testing';

import { KofZaProrPravPresekaService } from './kof-za-pror-prav-preseka.service';

describe('KofZaProrPravPresekaService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [KofZaProrPravPresekaService]
    });
  });

  it('should be created', inject([KofZaProrPravPresekaService], (service: KofZaProrPravPresekaService) => {
    expect(service).toBeTruthy();
  }));
});
