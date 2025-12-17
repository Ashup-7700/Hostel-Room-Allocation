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
export class FeeStructureComponent implements OnInit {

  api = 'http://localhost:5027/api/FeeStructure';

  list: any[] = [];
  form!: FormGroup;
  showModal = false;
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      feeStructureId: [0],
      roomType: ['', Validators.required],
      monthlyRent: [0, Validators.required],
      securityDeposit: [0, Validators.required],
      isActive: [true]
    });

    this.load();
  }

  get headers() {
    return { Authorization: `Bearer ${localStorage.getItem('token')}` };
  }

  // Load all Fee Structures
  load(): void {
    this.http.post<any>(`${this.api}/filter`, {}, { headers: this.headers })
      .subscribe(res => this.list = res?.data ?? []);
  }

  // Open modal to add new
  openAdd(): void {
    this.form.reset({ feeStructureId: 0, isActive: true });
    this.showModal = true;
  }

  // Open modal to edit existing
  openEdit(data: any): void {
    this.form.patchValue(data);
    this.showModal = true;
  }

  // Close modal
  close(): void {
    this.showModal = false;
    this.form.reset({ feeStructureId: 0 });
  }

  // Reset form
  reset(): void {
    this.form.reset({ feeStructureId: 0, isActive: true });
  }

  // Save or Update
  save(): void {
    if (this.form.invalid) return;

    const payload = {
      feeStructureId: this.form.value.feeStructureId || 0,
      roomType: this.form.value.roomType,
      monthlyRent: Number(this.form.value.monthlyRent),
      securityDeposit: Number(this.form.value.securityDeposit),
      isActive: this.form.value.isActive ?? true
    };

    this.loading = true;
    this.http.post(`${this.api}/addOrUpdate`, payload, { headers: this.headers })
      .subscribe({
        next: () => {
          this.loading = false;
          this.close();
          this.load();
        },
        error: err => {
          this.loading = false;
          console.error('Save error', err);
          alert('Failed to save Fee Structure');
        }
      });
  }

  // Delete Fee Structure
  delete(id: number): void {
    if (!confirm('Delete this Fee Structure?')) return;

    this.loading = true;
    this.http.delete(`${this.api}/${id}`, { headers: this.headers })
      .subscribe({
        next: () => {
          this.loading = false;
          alert('Deleted successfully');
          this.load();
        },
        error: err => {
          this.loading = false;
          console.error('Delete failed', err);
          alert('Delete failed');
        }
      });
  }

  // View details
  viewDetails(data: any): void {
    alert(
`Fee Structure Details:
Room Type: ${data.roomType}
Monthly Rent: ${data.monthlyRent}
Security Deposit: ${data.securityDeposit}
Status: ${data.isActive ? 'Active' : 'Inactive'}`
    );
  }

}
