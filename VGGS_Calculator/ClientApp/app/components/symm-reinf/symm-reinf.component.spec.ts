import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { } from 'jasmine';

import { SymmReinfComponent } from './symm-reinf.component';

describe('SymmReinfComponent', () => {
  let component: SymmReinfComponent;
  let fixture: ComponentFixture<SymmReinfComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SymmReinfComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SymmReinfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
