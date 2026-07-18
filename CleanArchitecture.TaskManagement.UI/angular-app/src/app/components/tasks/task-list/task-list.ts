import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskService } from '../../../services/task.service';
import { AuthService } from '../../../services/auth.service';
import { Task, CreateTask, UpdateTask } from '../../../models/task.model';
import { TaskCardComponent } from '../task-card/task-card';
import { TaskFormComponent } from '../task-form/task-form';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, TaskCardComponent, TaskFormComponent],
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
})
export class TaskListComponent implements OnInit {
  
  // All signals - Angular 20's reactive state
  tasks = signal<Task[]>([]);
  isLoading = signal(false);
  showForm = signal(false);
  filterCompleted = signal<boolean | null>(null);
  
  // Computed - automatically recalculates when tasks or filter changes
  filteredTasks = computed(() => {
    const filter = this.filterCompleted();
    const allTasks = this.tasks();
    
    if (filter === null) return allTasks;
    return allTasks.filter(t => t.isCompleted === filter);
  });
  completedCount = computed(() => 
    this.tasks().filter(t => t.isCompleted).length
  );
  pendingCount = computed(() => 
    this.tasks().filter(t => !t.isCompleted).length
  );
  constructor(
    private taskService: TaskService,
    public authService: AuthService
  ) {}
  ngOnInit() {
    this.loadTasks();
  }
  loadTasks() {
    this.isLoading.set(true);
    
    this.taskService.getAllTasks().subscribe({
      next: (tasks) => {
        this.tasks.set(tasks);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
  onTaskCreated(newTask: CreateTask) {
    this.taskService.createTask(newTask).subscribe({
      next: (task) => {
        // Add new task to beginning of list
        this.tasks.update(tasks => [task, ...tasks]);
        this.showForm.set(false);
      }
    });
  }
  onTaskUpdated(event: { id: number; data: UpdateTask }) {
    this.taskService.updateTask(event.id, event.data).subscribe({
      next: (updatedTask) => {
        this.tasks.update(tasks =>
          tasks.map(t => t.id === updatedTask.id ? updatedTask : t)
        );
      }
    });
  }
  onTaskDeleted(id: number) {
    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.tasks.update(tasks => tasks.filter(t => t.id !== id));
      }
    });
  }
  setFilter(value: boolean | null) {
    this.filterCompleted.set(value);
  }
}