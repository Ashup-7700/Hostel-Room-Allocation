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
  studentApi = 'http://localhost:5027/api/Student';

  list: any[] = [];
  students: any[] = [];

  form!: FormGroup;
  showModal = false;
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      paymentId: [0],
      studentId: [null, Validators.required],
      amount: [null, Validators.required],
      paymentMode: ['', Validators.required],
      paymentStatus: ['Pending']
    });

    this.loadPayments();
    this.loadStudents();
  }

  get headers() {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    };
  }

  /* ================= LOAD DATA ================= */

  loadPayments(): void {
    // Use getByFilter with empty filter to fetch all payments
    this.http.post<any>(
      `${this.api}/getByFilter`,
      {},
      { headers: this.headers }
    ).subscribe({
      next: res => {
        this.list = res?.data ?? [];
      },
      error: err => {
        console.error('Error loading payments', err);
      }
    });
  }

  loadStudents(): void {
    this.http.get<any>(
      `${this.studentApi}/getAll`,
      { headers: this.headers }
    ).subscribe({
      next: res => {
        this.students = res?.data ?? [];
      },
      error: err => {
        console.error('Error loading students', err);
      }
    });
  }

  /* ================= HELPERS ================= */

  getStudentName(studentId: number): string {
    const student = this.students.find(x => x.studentId === studentId);
    return student ? student.name : '-';
  }

  /* ================= MODAL ================= */

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
    alert(`Payment Details
Student: ${this.getStudentName(data.studentId)}
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

 save(): void {
  if (this.form.invalid) return;

  // Find the existing payment from the list if updating
  const existing = this.list.find(x => x.paymentId === this.form.value.paymentId);

  const payload = {
    paymentId: Number(this.form.value.paymentId) || 0,
    studentId: Number(this.form.value.studentId),
    amount: Number(this.form.value.amount),
    paymentMode: this.form.value.paymentMode,
    paymentStatus: this.form.value.paymentStatus || 'Pending',

    // Keep CreatedBy and CreatedByUserId from existing payment if updating
    createdBy: existing?.createdBy ?? null,
    createdByUserId: existing?.createdByUserId ?? null
  };

  console.log('Payload to backend:', payload);

  this.loading = true;

  this.http.post(`${this.api}/addOrUpdate`, payload, { headers: this.headers })
    .subscribe({
      next: () => {
        this.loading = false;
        this.close();
        this.loadPayments();
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
    ).subscribe({
      next: () => this.loadPayments(),
      error: err => console.error('Delete error', err)
    });
  }
}
