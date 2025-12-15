import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-fee-structure',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './fee-structure.html',
  styleUrls: ['./fee-structure.css']
})
export class FeeStructure implements OnInit {

  api = 'http://localhost:5027/api/FeeStructure';
  list: any[] = [];
  form!: FormGroup;
  loading = false;
  message = '';

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      feeStructureId: [0],
      roomType: ['', Validators.required],
      amount: ['', Validators.required],
      description: [''],
      isActive: [true]
    });

    this.load();
  }

  get headers() {
    return { Authorization: `Bearer ${localStorage.getItem('token')}` };
  }

  load(): void {
    this.loading = true;
    this.http.get<any>(`${this.api}/getAll`, { headers: this.headers })
      .subscribe({
        next: res => {
          this.list = res.data ?? [];
          this.loading = false;
        },
        error: () => {
          this.message = 'Failed to load fee structure';
          this.loading = false;
        }
      });
  }

  save(): void {
    if (this.form.invalid) return;

    this.loading = true;
    this.http.post(`${this.api}/addOrUpdate`, this.form.value, { headers: this.headers })
      .subscribe({
        next: () => {
          this.message = 'Fee structure saved successfully';
          this.form.reset({ feeStructureId: 0, isActive: true });
          this.load();
          this.loading = false;
        },
        error: () => {
          this.message = 'Failed to save fee structure';
          this.loading = false;
        }
      });
  }

  edit(fee: any): void {
    this.form.patchValue(fee);
    window.scroll({ top: 0, behavior: 'smooth' });
  }

  delete(id: number): void {
    if (!confirm('Are you sure you want to delete this fee structure?')) return;

    this.loading = true;
    this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
      .subscribe({
        next: () => {
          this.message = 'Fee structure deleted successfully';
          this.load();
          this.loading = false;
        },
        error: () => {
          this.message = 'Failed to delete fee structure';
          this.loading = false;
        }
      });
  }
}
