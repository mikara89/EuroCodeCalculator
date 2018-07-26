import { TestBed, inject } from '@angular/core/testing';

import { SavPravPresekaEc2Service } from './sav-prav-preseka-ec2.service';

describe('SavPravPresekaEc2Service', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SavPravPresekaEc2Service]
    });
  });

  it('should be created', inject([SavPravPresekaEc2Service], (service: SavPravPresekaEc2Service) => {
    expect(service).toBeTruthy();
  }));
});
