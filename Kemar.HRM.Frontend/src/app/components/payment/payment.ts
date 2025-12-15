import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './payment.html'
})
export class Payment implements OnInit {

  api = 'http://localhost:5027/api/Payment';
  list: any[] = [];
  form!: FormGroup;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      paymentId: [0],
      studentId: ['', Validators.required],
      amount: ['', Validators.required],
      paymentMode: ['', Validators.required],
      paymentStatus: ['Paid']
    });

    this.load();
  }

  get headers() {
    return { Authorization: `Bearer ${localStorage.getItem('token')}` };
  }

  load() {
    this.http.get<any>(`${this.api}/getAll`, { headers: this.headers })
      .subscribe(res => this.list = res.data ?? []);
  }

  save() {
    this.http.post(`${this.api}/addOrUpdate`, this.form.value, { headers: this.headers })
      .subscribe(() => {
        this.form.reset({ paymentId: 0 });
        this.load();
      });
  }

  delete(id: number) {
    this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
      .subscribe(() => this.load());
  }
}
