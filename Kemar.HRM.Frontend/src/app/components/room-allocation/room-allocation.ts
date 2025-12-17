import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-room-allocation',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './room-allocation.html',
  styleUrls: ['./room-allocation.css']
})
export class RoomAllocationComponent implements OnInit {

  api = 'http://localhost:5027/api/RoomAllocation';
  studentApi = 'http://localhost:5027/api/Student';
  roomApi = 'http://localhost:5027/api/Room';

  list: any[] = [];
  students: any[] = [];
  rooms: any[] = [];
  
  form!: FormGroup;
  showModal = false;
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      roomAllocationId: [0],
      studentId: ['', Validators.required],
      roomId: ['', Validators.required],
      allocatedAt: ['', Validators.required],
      releasedAt: [''],
      isActive: [true]
    });

    this.load();
    this.loadStudents();
    this.loadRooms();
  }

  get headers() {
    return { Authorization: `Bearer ${localStorage.getItem('token')}` };
  }

  /* ===================== LOADERS ===================== */

  load(): void {
    this.http.post<any>(`${this.api}/GetByFilter`, {}, { headers: this.headers })
      .subscribe(res => {
        this.list = (res.data ?? []).map((x: any) => ({
          ...x,
          allocatedAt: x.allocatedAt ? new Date(x.allocatedAt) : null,
          releasedAt: x.releasedAt ? new Date(x.releasedAt) : null
        }));
      });
  }

 loadStudents(): void {
  this.http.get<any>(`${this.studentApi}/getAll`, { headers: this.headers })
    .subscribe(res => {
      console.log('Students API:', res.data);
      this.students = res.data ?? [];
    });
}

  loadRooms(): void {
    this.http.post<any>(`${this.roomApi}/getByFilter`, {}, { headers: this.headers })
      .subscribe(res => this.rooms = res.data ?? []);
  }

  /* ===================== HELPER METHODS (✅ INSIDE CLASS) ===================== */

getStudentName(studentId: number): string {
  const s = this.students.find(x =>
    x.studentId === studentId ||
    x.studentID === studentId ||
    x.id === studentId
  );

  return s?.studentName || s?.name || s?.fullName || '-';
}


  getRoomNumber(roomId: number): string {
    return this.rooms.find(r => r.roomId === roomId)?.roomNumber ?? '-';
  }

viewDetails(data: any): void {
  const studentName = this.getStudentName(data.studentId);
  const roomNumber = this.getRoomNumber(data.roomId);

  alert(
`Room Allocation Details
Student Name : ${studentName}
Room Number  : ${roomNumber}
Allocated At: ${data.allocatedAt ? new Date(data.allocatedAt).toISOString().split('T')[0] : '-'}
Released At : ${data.releasedAt ? new Date(data.releasedAt).toISOString().split('T')[0] : '-'}
Status      : ${data.isActive ? 'Active' : 'Released'}`
  );
}

  /* ===================== MODAL ===================== */

  openAdd(): void {
    this.form.reset({ roomAllocationId: 0, isActive: true });
    this.showModal = true;
  }

  openEdit(data: any): void {
    this.showModal = true;
    this.form.patchValue({
      roomAllocationId: data.roomAllocationId,
      studentId: data.studentId,
      roomId: data.roomId,
      allocatedAt: data.allocatedAt
        ? new Date(data.allocatedAt).toISOString().split('T')[0]
        : '',
      releasedAt: data.releasedAt
        ? new Date(data.releasedAt).toISOString().split('T')[0]
        : '',
      isActive: data.isActive
    });
  }

  close(): void {
    this.showModal = false;
    this.form.reset();
  }

  reset(): void {
    this.form.reset({ roomAllocationId: 0, isActive: true });
  }

  save(): void {
    if (this.form.invalid) return;

    this.loading = true;
    this.http.post(`${this.api}/addOrUpdate`, this.form.value, { headers: this.headers })
      .subscribe(() => {
        this.loading = false;
        this.close();
        this.load();
      });
  }

  delete(id: number): void {
    if (!confirm('Delete allocation?')) return;
    this.http.delete(`${this.api}/Delete/${id}`, { headers: this.headers })
      .subscribe(() => this.load());
  }
}
