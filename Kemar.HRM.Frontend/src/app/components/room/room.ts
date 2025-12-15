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
export class Room implements OnInit {

  api = 'http://localhost:5027/api/Room';

  list: any[] = [];
  form!: FormGroup;
  loading = false;
  showModal = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      roomId: [0],
      roomNumber: ['', Validators.required],
      roomType: ['', Validators.required],
      floor: [0, Validators.required],
      capacity: [0, Validators.required],
      currentOccupancy: [0]
    });

    this.load();
  }

  get headers() {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    };
  }

  load(): void {
    const filter = {}; // empty filter = get all

    this.http.post<any>(`${this.api}/getByFilter`, filter, { headers: this.headers })
      .subscribe({
        next: res => this.list = res.data ?? [],
        error: err => console.error(err)
      });
  }

  openAdd(): void {
    this.form.reset({ roomId: 0, currentOccupancy: 0 });
    this.showModal = true;
  }

  openEdit(data: any): void {
    this.form.patchValue(data);
    this.showModal = true;
  }

  close(): void {
    this.showModal = false;
    this.form.reset({ roomId: 0 });
  }

  reset(): void {
    this.form.reset({ roomId: 0 });
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
