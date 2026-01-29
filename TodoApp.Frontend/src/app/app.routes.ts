import { Routes } from '@angular/router';
import { TodoListComponent } from './components/todo-list/todo-list';
import { LoginComponent } from './components/login/login';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, 
  { path: 'login', component: LoginComponent },        
  { path: 'todo-list', component: TodoListComponent }   
];