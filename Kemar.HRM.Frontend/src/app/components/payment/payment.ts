import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './payment.html',
  styleUrls: ['./payment.css']
})
export class Payment implements OnInit {

  api = 'http://localhost:5027/api/Payment';
  list: any[] = [];
  form!: FormGroup;
  showModal = false;
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      paymentId: [0],
      studentId: [null, Validators.required],
      amount: [null, Validators.required],
      paymentMode: ['', Validators.required],
      paymentStatus: ['Paid']
    });

    this.load();
  }

  get headers() {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    };
  }

  // ✅ MUST use getByFilter (matches backend)
  load(): void {
    this.http.post<any>(
      `${this.api}/getByFilter`,
      {},
      { headers: this.headers }
    ).subscribe({
      next: res => this.list = res?.data ?? [],
      error: err => console.error('Load error', err)
    });
  }

  openAdd(): void {
    this.form.reset({
      paymentId: 0,
      paymentStatus: 'Paid'
    });
    this.showModal = true;
  }

  openEdit(data: any): void {
    this.form.patchValue(data);
    this.showModal = true;
  }

  viewDetails(data: any): void {
    alert(`Payment Details:
Student ID: ${data.studentId}
Amount: ${data.amount}
Mode: ${data.paymentMode}
Status: ${data.paymentStatus}`);
  }

  close(): void {
    this.showModal = false;
    this.form.reset({ paymentId: 0 });
  }

  reset(): void {
    this.form.reset({
      paymentId: 0,
      paymentStatus: 'Paid'
    });
  }

  // ✅ IMPORTANT FIX: send clean numeric payload
  save(): void {
    if (this.form.invalid) return;

    const payload = {
      paymentId: this.form.value.paymentId || 0,
      studentId: Number(this.form.value.studentId),
      amount: Number(this.form.value.amount),
      paymentMode: this.form.value.paymentMode,
      paymentStatus: this.form.value.paymentStatus
    };

    this.loading = true;

    this.http.post(
      `${this.api}/addOrUpdate`,
      payload,
      { headers: this.headers }
    ).subscribe({
      next: () => {
        this.loading = false;
        this.close();
        this.load();
      },
      error: err => {
        console.error('Save error', err);
        this.loading = false;
      }
    });
  }

  delete(id: number): void {
    if (!confirm('Delete this payment?')) return;

    this.http.delete(
      `${this.api}/delete/${id}`,
      { headers: this.headers }
    ).subscribe(() => this.load());
  }
}
