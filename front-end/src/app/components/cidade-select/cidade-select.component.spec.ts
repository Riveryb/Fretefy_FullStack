import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CidadeSelectComponent } from './cidade-select.component';

describe('CidadeSelectComponent', () => {
  let component: CidadeSelectComponent;
  let fixture: ComponentFixture<CidadeSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CidadeSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CidadeSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
