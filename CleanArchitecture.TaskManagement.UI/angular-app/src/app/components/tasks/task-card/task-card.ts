import { Component, Input, Output, EventEmitter, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, UpdateTask } from '../../../models/task.model';

@Component({
  selector: 'app-task-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './task-card.html',
  styleUrl: './task-card.scss',
})

export class TaskCardComponent {
  
  @Input({ required: true }) task!: Task;
  
  @Output() taskUpdated = new EventEmitter<{ id: number; data: UpdateTask }>();
  @Output() taskDeleted = new EventEmitter<number>();
  isExpanded = signal(false);
  toggleComplete() {
    this.taskUpdated.emit({
      id: this.task.id,
      data: { isCompleted: !this.task.isCompleted }
    });
  }
  deleteTask() {
    if (confirm(`Delete "${this.task.title}"?`)) {
      this.taskDeleted.emit(this.task.id);
    }
  }
}