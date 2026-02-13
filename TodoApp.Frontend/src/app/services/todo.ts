import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Todo {
  id: string;
  title: string;
  description?: string;
  isCompleted: boolean;
}

export interface CreateTodoCommand {
  title: string;
  description?: string;
  userId: string;
}

export interface UpdateTodoCommand {
  id: string;
  title: string;
  description?: string;
  isCompleted: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private apiUrl = 'http://localhost:5157/api/Todo'; 

  constructor(private http: HttpClient) { }


  login(email: string, password: string): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/login`, { email, password });
  }

  register(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, { email, password });
  }

  getAll(userId: string): Observable<Todo[]> {
    return this.http.get<Todo[]>(`${this.apiUrl}?userId=${userId}`);
  }

  create(todo: CreateTodoCommand): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}`, todo);
  }

  update(todo: UpdateTodoCommand): Observable<void> {
    return this.http.put<void>(this.apiUrl, todo); 
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}