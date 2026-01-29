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

  constructor(
    private todoService: TodoService,
    private router: Router
  ) {}

  login() {
    if (!this.email.trim()) {
      alert("Please enter an email address!");
      return;
    }

    this.todoService.login(this.email).subscribe({
      next: (userId) => {
        console.log("Login successful, UserID:", userId);
        
        localStorage.setItem('userId', userId);
        
        this.router.navigate(['/todo-list']); 
      },
      error: (err) => {
        console.error("Login error:", err);
        alert("Login failed! Is the backend running?");
      }
    });
  }
}