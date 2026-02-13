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
  userId: string | null = null;
  today: Date = new Date();

  isDialogOpen = false;
  editingTodoId: string | null = null;
  
  currentTodo: any = { title: '', description: '' }; 

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

  openDialog(todo?: Todo) {
    this.isDialogOpen = true;
    if (todo) {
      this.editingTodoId = todo.id;
      this.currentTodo = { ...todo }; 
    } else {
      this.editingTodoId = null;
      this.currentTodo = { title: '', description: '' };
    }
  }

  closeDialog() {
    this.isDialogOpen = false;
    this.currentTodo = { title: '', description: '' };
    this.editingTodoId = null;
  }

  saveTodo() {
    if (!this.currentTodo.title.trim()) {
      alert("Title is required!");
      return;
    }

    if (this.editingTodoId) {
      const updateModel = {
        id: this.editingTodoId,
        title: this.currentTodo.title,
        description: this.currentTodo.description,
        isCompleted: this.currentTodo.isCompleted,
        userId: this.userId! 
      };
      
      this.todoService.update(updateModel).subscribe({
        next: () => {
          this.getTodos();
          this.closeDialog();
        },
        error: (err) => alert("Update failed!")
      });

    } else {
      const newModel = {
        title: this.currentTodo.title,
        description: this.currentTodo.description,
        userId: this.userId!
      };

      this.todoService.create(newModel).subscribe({
        next: () => {
          this.getTodos();
          this.closeDialog();
        },
        error: (err) => alert("Create failed!")
      });
    }
  }

  deleteTodo(id: string) {
    if (confirm("Are you sure you want to delete this task?")) {
      const backupList = [...this.todos];
      this.todos = this.todos.filter(t => t.id !== id); 
      
      this.todoService.delete(id).subscribe({
        error: (err) => {
          console.error("Deletion failed, reverting...", err);
          this.todos = backupList;
          alert("Failed to delete task.");
        }
      });
    }
  }

  toggleComplete(todo: Todo) {
    todo.isCompleted = !todo.isCompleted;
    
    const updateModel = {
      id: todo.id,
      title: todo.title,
      description: todo.description || "",
      isCompleted: todo.isCompleted,
      userId: this.userId!
    };

    this.todoService.update(updateModel).subscribe({
      error: (err) => {
        console.error("Error:", err);
        todo.isCompleted = !todo.isCompleted;
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