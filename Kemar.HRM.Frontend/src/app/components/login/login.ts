import { Component, OnInit } from '@angular/core';
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
export class LoginComponent implements OnInit {

  private apiUrl = 'http://localhost:5027/api/User';

  isLoginMode = true;
  showForgot = false;

  loading = false;
  message = '';

  loginForm: FormGroup;
  registerForm: FormGroup;
  forgotForm: FormGroup;

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

    this.forgotForm = this.fb.group({
      username: [''],
      email: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    if (localStorage.getItem('token')) {
      this.router.navigate(['/dashboard']);
    }
  }

login(): void {
  if (this.loginForm.invalid) return;

  this.loading = true;
  this.message = '';

  const payload = {
    username: this.loginForm.value.username.trim().toLowerCase(),
    password: this.loginForm.value.password
  };

  this.http.post<any>(`${this.apiUrl}/login`, payload)
    .subscribe({
      next: (res) => {
        this.loading = false;

        localStorage.setItem('token', res.token);
        localStorage.setItem('userId', res.userId);
        localStorage.setItem('username', res.username);
        localStorage.setItem('role', res.role);

        this.message = 'Login successful';
        setTimeout(() => this.router.navigate(['/dashboard']), 500);
      },
      error: (err) => {
        this.loading = false;

        const msg: string =
          err?.error?.message?.toLowerCase() || '';

        if (msg.includes('password')) {
          this.message = '❌ Password is incorrect';
        } 
        else if (msg.includes('username')) {
          this.message = '❌ Username is incorrect';
        } 
        else {
          this.message = '❌ Invalid username or password';
        }
      }
    });
}


  register(): void {
    if (this.registerForm.invalid) return;

    this.loading = true;
    this.message = '';
    
  const payload = {
    ...this.registerForm.value,
    username: this.registerForm.value.username.trim().toLowerCase()
  };

    this.http.post(`${this.apiUrl}/addOrUpdate`, this.registerForm.value)
      .subscribe({
        next: () => {
          this.loading = false;
          this.message = '✅ Registration successful. Please login.';
          this.registerForm.reset({ role: 'Admin' });
          this.isLoginMode = true;
        },
        error: (err) => {
          this.loading = false;
          this.message = err?.error?.message || '❌ Registration failed';
        }
      });
  }

  forgot(): void {
    if (this.forgotForm.invalid) return;

    this.loading = true;
    this.message = '';

    setTimeout(() => {
      this.loading = false;
      this.message =
        '✅ If the account exists, username/password details have been sent to your email.';
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
