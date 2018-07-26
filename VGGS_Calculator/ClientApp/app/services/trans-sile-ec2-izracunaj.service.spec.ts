import { TestBed, inject } from '@angular/core/testing';

import { TransSileEc2IzracunajService } from './trans-sile-ec2-izracunaj.service';

describe('TransSileEc2IzracunajService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TransSileEc2IzracunajService]
    });
  });

  it('should be created', inject([TransSileEc2IzracunajService], (service: TransSileEc2IzracunajService) => {
    expect(service).toBeTruthy();
  }));
});
