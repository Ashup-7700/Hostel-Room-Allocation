// import { Component, OnInit } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
// import { HttpClient, HttpClientModule } from '@angular/common/http';

// @Component({
//   selector: 'app-room',
//   standalone: true,
//   imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
//   templateUrl: './room.html',
//   styleUrls: ['./room.css']
// })
// export class RoomComponent implements OnInit {

//   api = 'http://localhost:5027/api/Room';

//   list: any[] = [];
//   form!: FormGroup;
//   showModal = false;
//   loading = false;

//   constructor(private fb: FormBuilder, private http: HttpClient) { }

//   ngOnInit(): void {
//     this.form = this.fb.group({
//       roomId: [0],
//       roomNumber: ['', Validators.required],
//       roomType: ['', Validators.required],
//       floor: [0, Validators.required],
//       capacity: [0, Validators.required],
//       currentOccupancy: [0],
//       isActive: [true]
//     });

//     this.load();
//   }

//   get headers() {
//     return { Authorization: `Bearer ${localStorage.getItem('token')}` };
//   }

//   load(): void {
//     this.http.post<any>(`${this.api}/GetByFilter`, {}, { headers: this.headers })
//       .subscribe(res => this.list = res.data ?? []);
//   }

//   openAdd(): void {
//     this.form.reset({
//       roomId: 0,
//       roomNumber: '',
//       roomType: '',
//       floor: 0,
//       capacity: 0,
//       currentOccupancy: 0,
//       isActive: true
//     });
//     this.showModal = true;
//   }

//   openEdit(data: any): void {
//     this.form.patchValue({
//       roomId: data.roomId,
//       roomNumber: data.roomNumber,
//       roomType: data.roomType,
//       floor: data.floor,
//       capacity: data.capacity,
//       currentOccupancy: data.currentOccupancy ?? 0,
//       isActive: data.isActive
//     });
//     this.showModal = true;
//   }

//   viewDetails(data: any): void {
//     alert(
// `Room Details
// Room No: ${data.roomNumber}
// Type: ${data.roomType}
// Floor: ${data.floor}
// Capacity: ${data.capacity}
// Occupancy: ${data.currentOccupancy}
// Status: ${data.isActive ? 'Active' : 'Inactive'}`
//     );
//   }

//   close(): void {
//     this.showModal = false;
//     this.form.reset();
//   }

//   reset(): void {
//     this.form.reset({
//       roomId: this.form.value.roomId || 0,
//       roomNumber: this.form.value.roomNumber || '',
//       roomType: this.form.value.roomType || '',
//       floor: this.form.value.floor || 0,
//       capacity: this.form.value.capacity || 0,
//       currentOccupancy: this.form.value.currentOccupancy || 0,
//       isActive: this.form.value.isActive ?? true
//     });
//   }

//   save(): void {
//     if (this.form.invalid) return;

//     const payload = {
//       roomId: this.form.value.roomId || 0,
//       roomNumber: this.form.value.roomNumber,
//       roomType: this.form.value.roomType,
//       floor: Number(this.form.value.floor),
//       capacity: Number(this.form.value.capacity),
//       currentOccupancy: Number(this.form.value.currentOccupancy) || 0,
//       isActive: this.form.value.isActive ?? true
//     };

//     this.loading = true;

//     this.http.post(`${this.api}/addOrUpdate`, payload, { headers: this.headers })
//       .subscribe({
//         next: () => {
//           this.loading = false;
//           this.close();
//           this.load(); // refresh table
//         },
//         error: (err) => {
//           this.loading = false;
//           console.error('Save error', err);
//           alert('Save failed! Please check your data.');
//         }
//       });
//   }

//   delete(id: number): void {
//     if (!confirm('Delete room?')) return;

//     this.loading = true;
//     this.http.delete(`${this.api}/delete/${id}`, { headers: this.headers })
//       .subscribe({
//         next: () => {
//           this.loading = false;
//           alert('Room deleted successfully');
//           this.load();
//         },
//         error: (err) => {
//           this.loading = false;
//           console.error('Delete failed', err);
//           alert('Delete failed!');
//         }
//       });
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
  selector: 'app-room',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, FormsModule],
  templateUrl: './room.html',
  styleUrls: ['./room.css']
})
export class RoomComponent implements OnInit {

  api = 'http://localhost:5027/api/Room';

  list: any[] = [];
  filteredList: any[] = [];

  filterStatus: 'all' | 'active' | 'inactive' = 'all';
  searchText = '';

  form!: FormGroup;
  showModal = false;
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      roomId: [0],
      roomNumber: ['', Validators.required],
      roomType: ['', Validators.required],
      floor: [0, Validators.required],
      capacity: [0, Validators.required],
      currentOccupancy: [0],
      isActive: [true]
    });

    this.load();
  }

  /* ================= HEADERS ================= */

  get headers() {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    };
  }

  /* ================= LOAD ================= */

  load(): void {
    this.http
      .post<any>(`${this.api}/GetByFilter`, {}, { headers: this.headers })
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

    // Search filter
    if (this.searchText.trim()) {
      const text = this.searchText.toLowerCase();
      data = data.filter(x =>
        (x.roomNumber ?? '').toLowerCase().includes(text) ||
        (x.roomType ?? '').toLowerCase().includes(text) ||
        String(x.floor ?? '').includes(text) ||
        String(x.capacity ?? '').includes(text)
      );
    }

    this.filteredList = data;
  }

  /* ================= MODAL ================= */

  openAdd(): void {
    this.form.reset({
      roomId: 0,
      currentOccupancy: 0,
      isActive: true
    });
    this.showModal = true;
  }

  openEdit(data: any): void {
    this.form.patchValue(data);
    this.showModal = true;
  }

  close(): void {
    this.showModal = false;
    this.form.reset();
  }

  reset(): void {
    this.form.reset({ roomId: 0, currentOccupancy: 0, isActive: true });
  }

  /* ================= SAVE ================= */

save(): void {
  if (this.form.invalid || this.loading) return;

  this.loading = true;

  this.http
    .post(`${this.api}/addOrUpdate`, this.form.value, { headers: this.headers })
    .subscribe({
      next: () => {
        this.loading = false;
        this.close();

        this.filterStatus = 'all'; 
        this.load();               
      },
      error: () => {
        this.loading = false;
      }
    });
}


  /* ================= DELETE (SOFT UI UPDATE) ================= */

  delete(id: number): void {
    if (!confirm('Are you sure you want to delete this room?')) return;

    this.http
      .delete(`${this.api}/delete/${id}`, { headers: this.headers })
      .subscribe({
        next: () => {
          const room = this.list.find(r => r.roomId === id);
          if (room) {
            room.isActive = false; 
          }
          this.applyFilter();
        },
        error: () => alert('Failed to delete room')
      });
  }

  /* ================= VIEW ================= */

  viewDetails(data: any): void {
    alert(
`Room Details
Room No: ${data.roomNumber}
Type: ${data.roomType}
Floor: ${data.floor}
Capacity: ${data.capacity}
Occupancy: ${data.currentOccupancy}
Status: ${data.isActive ? 'Active' : 'Inactive'}`
    );
  }
}
