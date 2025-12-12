import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomAllocation } from './room-allocation';

describe('RoomAllocation', () => {
  let component: RoomAllocation;
  let fixture: ComponentFixture<RoomAllocation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoomAllocation]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoomAllocation);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
