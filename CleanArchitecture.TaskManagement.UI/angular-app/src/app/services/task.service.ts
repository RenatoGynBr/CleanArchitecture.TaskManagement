import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Task, CreateTask, UpdateTask } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  
  private apiUrl = `${environment.apiUrl}/tasks`;
  constructor(private http: HttpClient) {}
  getAllTasks() {
    return this.http.get<Task[]>(this.apiUrl);
  }
  getTaskById(id: number) {
    return this.http.get<Task>(`${this.apiUrl}/${id}`);
  }
  createTask(task: CreateTask) {
    return this.http.post<Task>(this.apiUrl, task);
  }
  updateTask(id: number, task: UpdateTask) {
    return this.http.put<Task>(`${this.apiUrl}/${id}`, task);
  }
  deleteTask(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}