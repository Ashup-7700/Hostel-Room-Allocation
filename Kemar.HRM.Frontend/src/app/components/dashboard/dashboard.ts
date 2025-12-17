import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent implements OnInit {

  apiUrl = 'http://localhost:5027/api';
  
  // Dashboard data
  totalStudents = 0;
  availableRooms = 0;
  allocatedRooms = 0;
  vacantRooms = 0;
  pendingPayments = 0;
  totalFeeCollected = 0;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

loadDashboardData(): void {
  const headers = new HttpHeaders({
    'Authorization': `Bearer ${localStorage.getItem('token') || ''}`
  });

  // Students
  this.http.get<any>(`${this.apiUrl}/Student/getAll`, { headers }).subscribe(
    res => {
      const students = res?.data || [];
      this.totalStudents = students.length;
    },
    () => { this.totalStudents = 0; }
  );

  // Rooms
  this.http.post<any>(`${this.apiUrl}/Room/getByFilter`, {}, { headers }).subscribe(
    res => {
      const rooms = res?.data || [];
      this.availableRooms = rooms.filter((r: any) => r.isActive && !r.currentOccupancy).length;
      this.allocatedRooms = rooms.filter((r: any) => r.currentOccupancy > 0).length;
      this.vacantRooms = rooms.filter((r: any) => r.isActive && r.currentOccupancy === 0).length;
    },
    () => { this.availableRooms = this.allocatedRooms = this.vacantRooms = 0; }
  );

  // Payments
  this.http.post<any>(`${this.apiUrl}/Payment/getByFilter`, {}, { headers }).subscribe(
    res => {
      const payments = res?.data || [];
      this.pendingPayments = payments.filter((p: any) => !p.isPaid).length;
      this.totalFeeCollected = payments.filter((p: any) => p.isPaid)
        .reduce((sum: number, p: any) => sum + (p.amount || 0), 0);
    },
    () => { this.pendingPayments = this.totalFeeCollected = 0; }
  );
}

}