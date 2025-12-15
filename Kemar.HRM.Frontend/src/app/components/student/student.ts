import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-student',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './student.html',
  styleUrls: ['./student.css']
})
export class Student implements OnInit {

  api = 'http://localhost:5027/api/Student';

  list: any[] = [];
  form!: FormGroup;
  loading = false;
  message = '';
  showModal = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      studentId: [0],
      name: ['', Validators.required],
      gender: ['', Validators.required],
      phone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      dateOfAdmission: ['', Validators.required]
    });

    this.load();
  }

  get headers() {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    };
  }

  load(): void {
    this.http.get<any>(`${this.api}/getAll`, { headers: this.headers })
      .subscribe(res => this.list = res.data ?? []);
  }

  openAdd(): void {
    this.form.reset({ studentId: 0 });
    this.showModal = true;
  }

  openEdit(data: any): void {
    this.form.patchValue(data);
    this.showModal = true;
  }

  close(): void {
    this.showModal = false;
    this.form.reset({ studentId: 0 });
  }

  reset(): void {
    this.form.reset({ studentId: 0 });
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
    if (!confirm('Delete student?')) return;

    this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
      .subscribe(() => this.load());
  }
}
