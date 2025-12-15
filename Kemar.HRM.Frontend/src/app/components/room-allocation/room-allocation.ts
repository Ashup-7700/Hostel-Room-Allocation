import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-room-allocation',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './room-allocation.html',
  styleUrls: ['./room-allocation.css']
})
export class RoomAllocationComponent implements OnInit {

  api = 'http://localhost:5027/api/RoomAllocation';

  list: any[] = [];
  form!: FormGroup;
  showModal = false;
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      roomAllocationId: [0],
      studentId: ['', Validators.required],
      roomId: ['', Validators.required],
      allocatedAt: ['', Validators.required],
      releasedAt: [''],
      isActive: [true]
    });

    this.load();
  }

  get headers() {
    return { Authorization: `Bearer ${localStorage.getItem('token')}` };
  }

  load(): void {
    this.http.post<any>(`${this.api}/GetByFilter`, {}, { headers: this.headers })
      .subscribe(res => {
        this.list = (res.data ?? []).map((x: any) => ({
          ...x,
          allocatedAt: x.allocatedAt ? new Date(x.allocatedAt) : null,
          releasedAt: x.releasedAt ? new Date(x.releasedAt) : null
        }));
      });
  }

  openAdd(): void {
    this.form.reset({
      roomAllocationId: 0,
      isActive: true
    });
    this.showModal = true;
  }

 openEdit(data: any) {
  this.showModal = true;

  this.form.patchValue({
    roomAllocationId: data.roomAllocationId,
    studentId: data.studentId,
    roomId: data.roomId,

    allocatedAt: data.allocatedAt
      ? new Date(data.allocatedAt).toISOString().split('T')[0]
      : '',

    releasedAt: data.releasedAt
      ? new Date(data.releasedAt).toISOString().split('T')[0]
      : '',

    isActive: data.isActive
  });
}


  close(): void {
    this.showModal = false;
    this.form.reset();
  }

  reset(): void {
    this.form.reset({ roomAllocationId: 0, isActive: true });
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
    if (!confirm('Delete allocation?')) return;

    this.http.delete(`${this.api}/Delete/${id}`, { headers: this.headers })
      .subscribe(() => this.load());
  }
}
