import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoModelComponent } from './info-model.component';

describe('InfoModelComponent', () => {
  let component: InfoModelComponent;
  let fixture: ComponentFixture<InfoModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InfoModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
