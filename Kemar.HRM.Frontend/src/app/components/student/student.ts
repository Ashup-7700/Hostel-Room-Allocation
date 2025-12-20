// import { Component, OnInit } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
// import { HttpClient, HttpClientModule } from '@angular/common/http';

// @Component({
//   selector: 'app-student',
//   standalone: true,
//   imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
//   templateUrl: './student.html',
//   styleUrls: ['./student.css']
// })
// export class StudentComponent implements OnInit {

//   api = 'http://localhost:5027/api/Student';

//   list: any[] = [];
//   form!: FormGroup;
//   showModal = false;
//   loading = false;

//   constructor(private fb: FormBuilder, private http: HttpClient) {}

//   ngOnInit(): void {
//     this.form = this.fb.group({
//       studentId: [0],
//       name: ['', Validators.required],
//       gender: ['', Validators.required],
//       phone: ['', Validators.required],
//       email: ['', [Validators.required, Validators.email]],
//       address: ['', Validators.required],
//       dateOfAdmission: ['', Validators.required],
//       isActive: [true]
//     });

//     this.load();
//   }

//   get headers() {
//     return { Authorization: `Bearer ${localStorage.getItem('token')}` };
//   }

//   load(): void {
//   this.http.get<any>(`${this.api}/getAll`, { headers: this.headers })
//     .subscribe(res => this.list = res.data ?? []);
// }

//   openAdd(): void {
//     this.form.reset({ studentId: 0, isActive: true });
//     this.showModal = true;
//   }

//   openEdit(data: any): void {
//     this.form.patchValue({
//       ...data,
//       dateOfAdmission: data.dateOfAdmission
//         ? new Date(data.dateOfAdmission).toISOString().split('T')[0]
//         : ''
//     });
//     this.showModal = true;
//   }

//   viewDetails(data: any): void {
//     alert(
// `Student Details
// Name: ${data.name}
// Gender: ${data.gender}
// Phone: ${data.phone}
// Email: ${data.email}
// Admission Date: ${data.dateOfAdmission}
// Status: ${data.isActive ? 'Active' : 'Inactive'}`
//     );
//   }

//   close(): void {
//     this.showModal = false;
//     this.form.reset();
//   }
//     reset(): void {
//     this.form.reset({ studentId: 0, isActive: true });
//   }

//   save(): void {
//     if (this.form.invalid) return;

//     this.loading = true;
//     this.http.post(`${this.api}/addOrUpdate`, this.form.value, { headers: this.headers })
//       .subscribe({
//         next: () => {
//           this.loading = false;
//           this.close();
//           this.load();
//         },
//         error: () => this.loading = false
//       });
//   }

//   delete(id: number): void {
//     if (!confirm('Delete student?')) return;

//     this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
//       .subscribe(() => this.load());
//   }

// }




































import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  FormsModule
} from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-student',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, FormsModule],
  templateUrl: './student.html',
  styleUrls: ['./student.css']
})
export class StudentComponent implements OnInit {

  api = 'http://localhost:5027/api/Student';

  list: any[] = [];
  filteredList: any[] = [];

  filterStatus: 'all' | 'active' | 'inactive' = 'all';
  searchText = '';

  form!: FormGroup;
  showModal = false;
  loading = false;
  students: any;

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      studentId: [0],
      name: ['', Validators.required],
      gender: ['', Validators.required],
      phone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      dateOfAdmission: ['', Validators.required],
      isActive: [true]
    });

    this.load();
  }

  get headers() {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    };
  }

  /* ================= LOAD ================= */

  load(): void {
    this.http
      .get<any>(`${this.api}/getAll`, { headers: this.headers })
      .subscribe(res => {
        this.list = res.data ?? [];
        this.applyFilter();
      });
  }



  /* ================= FILTER ================= */

  applyFilter(): void {
    let data = [...this.list];

    // Status filter
    if (this.filterStatus === 'active') {
      data = data.filter(x => x.isActive === true);
    } else if (this.filterStatus === 'inactive') {
      data = data.filter(x => x.isActive === false);
    }

    // Global search
    if (this.searchText.trim()) {
      const text = this.searchText.toLowerCase();
      data = data.filter(x =>
        (x.name ?? '').toLowerCase().includes(text) ||
        (x.gender ?? '').toLowerCase().includes(text) ||
        (x.phone ?? '').toLowerCase().includes(text) ||
        (x.email ?? '').toLowerCase().includes(text) ||
        (x.address ?? '').toLowerCase().includes(text) ||
        (x.dateOfAdmission ?? '').toLowerCase().includes(text)
      );
    }

    this.filteredList = data;
  }

  /* ================= MODAL ================= */

  openAdd(): void {
    this.form.reset({ studentId: 0, isActive: true });
    this.showModal = true;
  }

  openEdit(data: any): void {
    this.form.patchValue({
      ...data,
      dateOfAdmission: data.dateOfAdmission
        ? new Date(data.dateOfAdmission).toISOString().split('T')[0]
        : ''
    });
    this.showModal = true;
  }

  close(): void {
    this.showModal = false;
    this.form.reset();
  }

  reset(): void {
    this.form.reset({ studentId: 0, isActive: true });
  }

  /* ================= SAVE ================= */

  save(): void {
    if (this.form.invalid || this.loading) return;

    this.loading = true;

    this.http.post(`${this.api}/addOrUpdate`, this.form.value, { headers: this.headers })
      .subscribe({
        next: () => {
          this.loading = false;
          this.close();

          this.filterStatus = 'all'; // 🔥 IMPORTANT
          this.load();
        },
        error: () => this.loading = false
      });
  }


  /* ================= DELETE (SOFT) ================= */

// delete(id: number): void {
//   if (!confirm('Are you sure you want to delete this student?')) return;

//   this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
//     .subscribe({
//       next: () => {
//         const student = this.students.find((s: any) => s.studentId === id);
//         if (student) {
//           student.isActive = false; // show as inactive
//         }
//       },
//       error: () => alert('Failed to delete student')
//     });
// }




// delete(id: number): void {
//   if (!confirm('Are you sure you want to permanently delete this student?')) {
//     return;
//   }

//   this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
//     .subscribe({
//       next: () => {
//         // 🔥 REMOVE FROM MASTER LIST
//         this.list = this.list.filter((s: any) => s.studentId !== id);

//         // 🔥 UPDATE UI
//         this.applyFilter();
//       },
//       error: () => {
//         alert('Failed to permanently delete student');
//       }
//     });
// }


  /* ================= VIEW ================= */

  viewDetails(data: any): void {
    alert(
      `Student Details
Name: ${data.name}
Gender: ${data.gender}
Phone: ${data.phone}
Email: ${data.email}
Admission Date: ${data.dateOfAdmission}
Status: ${data.isActive ? 'Active' : 'Inactive'}`
    );
  }
}
