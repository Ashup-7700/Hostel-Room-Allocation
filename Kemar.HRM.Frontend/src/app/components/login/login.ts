import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import {
  HttpClient,
  HttpClientModule,
  HttpErrorResponse
} from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {

  private apiUrl = 'http://localhost:5027/api/User';

  loading = false;
  message = '';

  isLoginMode = true;
  showForgot = false;

  loginForm!: FormGroup;
  registerForm!: FormGroup;
  forgotForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {

    if (localStorage.getItem('token')) {
      this.router.navigate(['/dashboard']);
      return;
    }

    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required],
      role: ['Admin', Validators.required]
    });

    this.forgotForm = this.fb.group({
      username: [''],
      email: ['', Validators.required]
    });
  }

  /* ================= LOGIN ================= */
login(): void {
  if (this.loginForm.invalid) return;

  this.loading = true;
  this.message = '';

  const payload = {
    username: this.loginForm.value.username.trim(),
    password: this.loginForm.value.password
  };

  this.http.post<any>(`${this.apiUrl}/login`, payload)
    .subscribe({
      next: (res) => {
        this.loading = false;

        if (!res?.token) {
          this.message = '❌ Login failed';
          return;
        }

        localStorage.setItem('token', res.token);
        localStorage.setItem('userId', res.userId);
        localStorage.setItem('username', res.username);
        localStorage.setItem('role', res.role);

        this.message = '✅ Login successful';
        setTimeout(() => this.router.navigate(['/dashboard']), 300);
      },
      error: (err) => {
        this.loading = false;
        console.error('LOGIN ERROR:', err);
        this.message = err?.error?.message || '❌ Invalid username or password';
      }
    });
}


  /* ================= REGISTER ================= */

register(): void {
  if (this.registerForm.invalid) return;

  this.loading = true;
  this.message = '';

  const payload = {
    id: 0, // REQUIRED
    username: this.registerForm.value.username.trim(),
    password: this.registerForm.value.password,
    email: this.registerForm.value.username.trim() + '@gmail.com', // TEMP SAFE VALUE
    role: this.registerForm.value.role,
    isActive: true
  };

  this.http.post<any>(`${this.apiUrl}/addOrUpdate`, payload)
    .subscribe({
      next: (res) => {
        this.loading = false;

        if (res?.isSuccess) {
          this.message = '✅ Registration successful. Please login.';
          this.registerForm.reset({ role: 'Admin' });
          this.isLoginMode = true;
        } else {
          this.message = res?.message || '❌ Registration failed';
        }
      },
      error: (err) => {
        this.loading = false;
        console.error('REGISTER ERROR:', err);
        this.message = err?.error?.message || '❌ Registration failed';
      }
    });
}


  /* ================= FORGOT ================= */

  forgot(): void {
    if (this.forgotForm.invalid) return;

    this.loading = true;
    this.message = '';

    setTimeout(() => {
      this.loading = false;
      this.message =
        '✅ If the account exists, details have been sent to your email.';
      this.showForgot = false;
    }, 1000);
  }

  toggleMode(): void {
    this.isLoginMode = !this.isLoginMode;
    this.showForgot = false;
    this.message = '';
  }

  toggleForgot(): void {
    this.showForgot = !this.showForgot;
    this.message = '';
  }

  logout(): void {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
