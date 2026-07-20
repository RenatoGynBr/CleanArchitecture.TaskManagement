import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class RegisterComponent {

  // Signals for reactive state
  isLoading = signal(false);
  errorMessage = signal('');
  registerForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { }

  // initialize form after FormBuilder is injected
  ngOnInit() {
    this.registerForm = this.fb.group({
      fullname: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) return;
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.authService.register(this.registerForm.value as any).subscribe({
      next: () => {
        this.router.navigate(['/tasks']);
      },
      error: (err) => {
        this.errorMessage.set(
          err.error?.message || 'Registration failed. Please try again.'
        );
        this.isLoading.set(false);
      }
    });
  }
  // Getter helpers for template
  get emailControl() { return this.registerForm.get('email'); }
  get passwordControl() { return this.registerForm.get('password'); }
  get fullnameControl() { return this.registerForm.get('fullname'); }

}
