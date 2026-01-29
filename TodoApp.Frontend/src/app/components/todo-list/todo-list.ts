import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID, NgZone } from '@angular/core'; 
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TodoService, Todo } from '../../services/todo';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo-list.html',
  styleUrl: './todo-list.scss'
})
export class TodoListComponent implements OnInit {
  todos: Todo[] = [];
  newTodo = { title: '', description: '', isCompleted: false };
  
  userId: string | null = null;
  today: Date = new Date();

  editingTodoId: string | null = null;
  originalTodoData: Todo | null = null;

  constructor(
    private todoService: TodoService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private zone: NgZone, 
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.userId = localStorage.getItem('userId');
      if (!this.userId) {
        this.router.navigate(['/login']);
        return;
      }
      this.getTodos();
    }
  }

  getTodos() {
    if (!this.userId) return;
    
    this.todoService.getAll(this.userId).subscribe({
      next: (res) => {
        this.zone.run(() => {
          this.todos = res;
          this.cdr.detectChanges();
        });
      },
      error: (err) => console.error("Failed to load list:", err) 
    });
  }

  addTodo() {
    if (!this.newTodo.title.trim() || !this.userId) {
      alert("Please enter a title!"); 
      return;
    }

    const todoToSend = {
      title: this.newTodo.title,
      description: this.newTodo.description,
      userId: this.userId
    };

    this.todoService.create(todoToSend).subscribe({
      next: (res) => {
        this.newTodo = { title: '', description: '', isCompleted: false };
        this.getTodos(); 
      },
      error: (err) => console.error("Error adding task:", err) 
    });
  }

  deleteTodo(id: string) {
    if (confirm("Are you sure you want to delete this task?")) {
      const backupList = [...this.todos];
      this.todos = this.todos.filter(t => t.id !== id);
      this.cdr.detectChanges();

      this.todoService.delete(id).subscribe({
        error: (err) => {
          console.error("Deletion failed, reverting...", err); 
          this.todos = backupList;
          this.cdr.detectChanges();
          alert("Failed to delete task."); 
        }
      });
    }
  }

  startEdit(todo: Todo) {
    if (this.editingTodoId) this.cancelEdit();
    this.editingTodoId = todo.id;
    this.originalTodoData = { ...todo };
    this.cdr.detectChanges();
  }

  cancelEdit() {
    if (this.editingTodoId && this.originalTodoData) {
      const index = this.todos.findIndex(t => t.id === this.editingTodoId);
      if (index > -1) {
        this.todos[index] = { ...this.originalTodoData };
      }
    }
    this.editingTodoId = null;
    this.originalTodoData = null;
    this.cdr.detectChanges();
  }

  saveEdit(todo: Todo) {
    if (!todo.title.trim()) {
      alert("Title cannot be empty!"); 
      return;
    }

    this.editingTodoId = null;
    this.originalTodoData = null;
    this.cdr.detectChanges();

    this.todoService.update(todo).subscribe({
      error: (err) => {
        console.error("Update error:", err); 
        alert("Failed to save!"); 
        this.startEdit(todo);
      }
    });
  }

  toggleComplete(todo: Todo) {
    if (this.editingTodoId === todo.id) return;

    todo.isCompleted = !todo.isCompleted;
    this.cdr.detectChanges();

    const updateModel = {
      id: todo.id,
      title: todo.title,
      description: todo.description || "",
      isCompleted: todo.isCompleted,
      userId: this.userId
    };

    this.todoService.update(updateModel).subscribe({
      next: () => console.log("Success"), 
      error: (err) => {
        console.error("Error:", err);
        todo.isCompleted = !todo.isCompleted;
        this.cdr.detectChanges();
        alert("Update failed."); 
      }
    });
  }

  logout() {
    if (confirm("Are you sure you want to logout?")) {
      if (isPlatformBrowser(this.platformId)) {
        localStorage.removeItem('userId');
        this.router.navigate(['/login']);
      }
    }
  }

  trackByFn(index: number, item: Todo): string {
    return item.id;
  }
}