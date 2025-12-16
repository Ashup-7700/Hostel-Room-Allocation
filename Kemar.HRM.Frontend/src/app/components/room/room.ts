import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-room',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './room.html',
  styleUrls: ['./room.css']
})
export class RoomComponent implements OnInit {

  api = 'http://localhost:5027/api/Room';

  list: any[] = [];
  form!: FormGroup;
  showModal = false;
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      roomId: [0],
      roomNumber: ['', Validators.required],
      roomType: ['', Validators.required],
      floor: ['', Validators.required],
      capacity: ['', Validators.required],
      currentOccupancy: [0],
      isActive: [true]
    });

    this.load();
  }

  get headers() {
    return { Authorization: `Bearer ${localStorage.getItem('token')}` };
  }

  load(): void {
    this.http.post<any>(`${this.api}/GetByFilter`, {}, { headers: this.headers })
      .subscribe(res => this.list = res.data ?? []);
  }

  openAdd(): void {
    this.form.reset({
      roomId: 0,
      currentOccupancy: 0,
      isActive: true
    });
    this.showModal = true;
  }

  openEdit(data: any): void {
    this.form.patchValue(data);
    this.showModal = true;
  }

  viewDetails(data: any): void {
    alert(
`Room Details
Room No: ${data.roomNumber}
Type: ${data.roomType}
Floor: ${data.floor}
Capacity: ${data.capacity}
Occupancy: ${data.currentOccupancy}
Status: ${data.isActive ? 'Active' : 'Inactive'}`
    );
  }

  close(): void {
    this.showModal = false;
    this.form.reset();
  }
  

    reset(): void {
    this.form.reset({ roomId: 0, isActive: true });
  }

  save(): void {
    if (this.form.invalid) return;

    this.loading = true;
    this.http.post(`${this.api}/addOrUpdate`, this.form.value, { headers: this.headers })
      .subscribe({
        next: () => {
          this.loading = false;
          this.close();
          this.load();
        },
        error: () => this.loading = false
      });
  }

  delete(id: number): void {
    if (!confirm('Delete room?')) return;

    this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
      .subscribe(() => this.load());
  }
}
