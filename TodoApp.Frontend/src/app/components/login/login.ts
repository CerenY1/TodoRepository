import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TodoService } from '../../services/todo';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  isRegisterMode: boolean = false;

  constructor(private todoService: TodoService, private router: Router) {}

  toggleMode() {
    this.isRegisterMode = !this.isRegisterMode;
  }

  submit() {
    if (!this.email || !this.password) {
      alert("Please fill in all fields!");
      return;
    }
    const emailPattern = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
    if (!emailPattern.test(this.email)) {
      alert("Invalid email format!");
      return;
    }

    if (this.isRegisterMode) {
      this.todoService.register(this.email, this.password).subscribe({
        next: () => {
          alert("Registration successful! You can now log in.");
          this.isRegisterMode = false;
        },
        error: (err) => alert("Registration failed. This email might already be in use.")
      });
    } else {
      this.todoService.login(this.email, this.password).subscribe({
        next: (res: any) => { 
          const userId = typeof res === 'object' ? res.userId : res; 
          localStorage.setItem('userId', userId);
          this.router.navigate(['/todo-list']);
        },
        error: () => alert("Login failed! Incorrect email or password.")
      });
    }
  }
}