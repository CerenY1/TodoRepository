import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class AppComponent implements OnInit {
  todos: any[] = [];
  newTodo = { title: '', description: '', isCompleted: false };
  apiUrl = 'http://localhost:5157/api/todo';
  today: Date = new Date(); 

  editingTodoId: string | null = null;
  originalTodoData: any = null;

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit() { this.getTodos(); }

  getTodos() {
    this.http.get<any[]>(this.apiUrl).subscribe(res => {
      this.todos = res;
      this.cdr.detectChanges();
    });
  }

  addTodo() {
    if (!this.newTodo.title) {
      alert("Please enter a title!");
      return;
    }

    this.http.post<any>(this.apiUrl, this.newTodo).subscribe({
      next: (savedTodo) => {
        this.todos = [...this.todos, savedTodo];
        this.newTodo = { title: '', description: '', isCompleted: false }; 
        this.cdr.detectChanges(); 
      },
      error: (err) => console.error("Error:", err)
    });
  }

  deleteTodo(id: string) {
    if (confirm("Are you sure you want to delete this task?")) {
      const backupList = [...this.todos]; 
      this.todos = this.todos.filter(t => t.id !== id);
      this.cdr.detectChanges(); 

      this.http.delete(`${this.apiUrl}/${id}`).subscribe({
        error: (err) => {
          console.error("Delete failed, reverting...", err);
          this.todos = backupList;
          this.cdr.detectChanges();
          alert("Could not delete task.");
        }
      });
    }
  }

  startEdit(todo: any) {
    if (this.editingTodoId) this.cancelEdit();
    this.editingTodoId = todo.id;
    this.originalTodoData = { ...todo };
    this.cdr.detectChanges();
  }

  cancelEdit() {
    if (this.editingTodoId && this.originalTodoData) {
      const index = this.todos.findIndex(t => t.id === this.editingTodoId);
      if (index > -1) this.todos[index] = { ...this.originalTodoData };
    }
    this.editingTodoId = null;
    this.originalTodoData = null;
    this.cdr.detectChanges();
  }

  saveEdit(todo: any) {
    if (!todo.title) {
      alert("Title cannot be empty!");
      return;
    }

    this.editingTodoId = null;
    this.originalTodoData = null;
    this.cdr.detectChanges(); 

    this.http.put(`${this.apiUrl}/${todo.id}`, todo).subscribe({
      error: (err) => {
        console.error("Update error:", err);
        alert("Failed to save changes. Please try again.");
        this.startEdit(todo); 
      }
    });
  }

  toggleComplete(todo: any) {
    if (this.editingTodoId === todo.id) return;

    todo.isCompleted = !todo.isCompleted;
    this.cdr.detectChanges(); 

    this.http.put(`${this.apiUrl}/${todo.id}`, todo).subscribe({
      error: (err) => {
        todo.isCompleted = !todo.isCompleted;
        this.cdr.detectChanges();
        alert("Connection error.");
      }
    });
  }
}