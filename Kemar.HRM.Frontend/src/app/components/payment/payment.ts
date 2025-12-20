import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, FormsModule],
  templateUrl: './payment.html',
  styleUrls: ['./payment.css']
})
export class Payment implements OnInit {

  api = 'http://localhost:5027/api/Payment';
  studentApi = 'http://localhost:5027/api/Student';

  list: any[] = [];
  filteredList: any[] = [];
  students: any[] = [];

  filterStatus: 'all' | 'pending' | 'completed' = 'all';
  searchText = '';

  form!: FormGroup;
  showModal = false;
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit(): void {  
    this.form = this.fb.group({
  paymentId: [0],
  studentId: [null, Validators.required],
  paidAmount: [0, Validators.required],
  totalAmount: [0, Validators.required],  // now editable
  paymentMode: ['', Validators.required]
});


    this.loadPayments();
    this.loadStudents();

    // Update remaining dynamically when paidAmount changes
    this.form.get('paidAmount')?.valueChanges.subscribe(() => {
      const total = this.form.get('totalAmount')?.value || 0;
      this.form.patchValue({ remainingAmount: total - this.form.get('paidAmount')?.value }, { emitEvent: false });
    });
  }

  get headers() {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    };
  }

  /* ================= LOAD DATA ================= */
  loadPayments(): void {
    this.http.post<any>(`${this.api}/getByFilter`, {}, { headers: this.headers })
      .subscribe({
        next: res => {
          this.list = res?.data ?? [];
          this.applyFilter();
        },
        error: err => console.error('Error loading payments', err)
      });
  }

  loadStudents(): void {
    this.http.get<any>(`${this.studentApi}/getAll`, { headers: this.headers })
      .subscribe({
        next: res => this.students = res?.data ?? [],
        error: err => console.error('Error loading students', err)
      });
  }

  /* ================= HELPERS ================= */
  getStudentName(studentId: number): string {
    const student = this.students.find(x => x.studentId === studentId);
    return student ? student.name : '-';
  }

  getPaymentStatus(payment: any): string {
    return this.getRemainingAmount(payment) <= 0 ? 'Completed' : 'Pending';
  }

  getRemainingAmount(payment: any): number {
    return (payment.totalAmount || 0) - (payment.paidAmount || 0);
  }

  /* ================= FILTER ================= */
  applyFilter(): void {
    let data = [...this.list];

    if (this.filterStatus === 'pending') {
      data = data.filter(p => this.getRemainingAmount(p) > 0);
    } else if (this.filterStatus === 'completed') {
      data = data.filter(p => this.getRemainingAmount(p) <= 0);
    }

    if (this.searchText.trim()) {
      const text = this.searchText.toLowerCase();
      data = data.filter(p =>
        this.getStudentName(p.studentId).toLowerCase().includes(text) ||
        String(p.paidAmount).includes(text) ||
        String(p.totalAmount).includes(text) ||
        (p.paymentMode ?? '').toLowerCase().includes(text)
      );
    }

    this.filteredList = data;
  }

  /* ================= MODAL ================= */
  openAdd(): void {
    this.form.reset({ paymentId: 0, totalAmount: 0 });
    this.showModal = true;
  }

  openEdit(data: any): void {
    this.form.patchValue({
      paymentId: data.paymentId,
      studentId: data.studentId,
      paidAmount: data.paidAmount,
      totalAmount: data.totalAmount,
      paymentMode: data.paymentMode
    });
    this.showModal = true;
  }

  viewDetails(data: any): void {
    alert(`Payment Details
Student: ${this.getStudentName(data.studentId)}
Paid Amount: ${data.paidAmount}
Total Amount: ${data.totalAmount}
Remaining Amount: ${this.getRemainingAmount(data)}
Mode: ${data.paymentMode}
Status: ${this.getPaymentStatus(data)}`);
  }

  close(): void {
    this.showModal = false;
    this.form.reset({ paymentId: 0, totalAmount: 0 });
  }

  reset(): void {
    this.form.reset({ paymentId: 0, totalAmount: 0 });
  }

  /* ================= SAVE ================= */
  save(): void {
    if (this.form.invalid) return;

    const payload = {
      paymentId: Number(this.form.value.paymentId) || 0,
      studentId: Number(this.form.value.studentId),
      paidAmount: Number(this.form.value.paidAmount),
      totalAmount: Number(this.form.value.totalAmount),
      paymentMode: this.form.value.paymentMode
    };

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

    this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
      .subscribe({
        next: () => this.loadPayments(),
        error: err => console.error('Delete error', err)
      });
  }

  /* ================= DYNAMIC TOTAL ON STUDENT CHANGE ================= */
onStudentChange(studentId: string | number): void {
  const id = Number(studentId);
  const student = this.students.find(s => s.studentId === id);
  if (student) {
    const total = student.totalAmount ?? 0; // replace with actual total amount property
    this.form.patchValue({ totalAmount: total });
  }
}


}
