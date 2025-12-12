import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent {

  loginForm: FormGroup;
  registerForm: FormGroup;
  isLoginMode = true;
  loading = false;
  message = '';
  apiUrl = 'http://localhost:5027/api/User';

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
      role: ['', Validators.required]
    });
  }

  login() {
    if (this.loginForm.invalid) return;

    this.loading = true;
    this.message = '';

    this.http.post(`${this.apiUrl}/login`, this.loginForm.value)
      .subscribe({
        next: (res: any) => {
          this.loading = false;

          // Store user data
          localStorage.setItem('token', res.token);
          localStorage.setItem('username', res.username);
          localStorage.setItem('role', res.role);

          this.message = "Login Successful! Redirecting...";

          // Redirect to dashboard
          setTimeout(() => {
            this.router.navigate(['/dashboard']);
          }, 800);
        },
        error: () => {
          this.loading = false;
          this.message = "Invalid username or password.";
        }
      });
  }

  register() {
    if (this.registerForm.invalid) return;

    this.loading = true;
    this.message = '';

    this.http.post(`${this.apiUrl}/addOrUpdate`, this.registerForm.value)
      .subscribe({
        next: () => {
          this.loading = false;
          this.message = "User Registered Successfully!";
          this.registerForm.reset();
        },
        error: () => {
          this.loading = false;
          this.message = "Registration failed.";
        }
      });
  }

  toggleMode() {
    this.isLoginMode = !this.isLoginMode;
    this.message = '';
  }
}
