import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  private apiUrl = 'http://localhost:5027/api/User';

  isLoginMode = true;
  loading = false;
  message = '';

  loginForm: FormGroup;
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {

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
  }

login(): void {
  if (this.loginForm.invalid) return;

  console.log('Login Data:', this.loginForm.value); 

  this.loading = true;
  this.message = '';

  this.http.post<any>(`${this.apiUrl}/login`, this.loginForm.value)
    .subscribe({
      next: (res) => {
        console.log('Login Response:', res); 

        this.loading = false;

        localStorage.setItem('token', res.token);
        localStorage.setItem('userId', res.userId);
        localStorage.setItem('username', res.username);
        localStorage.setItem('role', res.role);

        this.message = 'Login successful.';
        setTimeout(() => this.router.navigate(['/dashboard']), 700);
      },
      error: (err) => {
        console.error('Login Error:', err); 
        this.loading = false;
        this.message = err?.error?.message || 'Invalid username or password';
      }
    });
}

  register(): void {
    if (this.registerForm.invalid) return;

    this.loading = true;
    this.message = '';

    this.http.post(`${this.apiUrl}/addOrUpdate`, this.registerForm.value)
      .subscribe({
        next: () => {
          this.loading = false;
          this.message = 'User registered successfully. Please login.';
          this.registerForm.reset({ role: 'Admin' });
          this.isLoginMode = true;
        },
        error: (err) => {
          this.loading = false;
          this.message = err?.error?.message || 'Registration failed ';
        }
      });
  }

  toggleMode(): void {
    this.isLoginMode = !this.isLoginMode;
    this.message = '';
  }
}
