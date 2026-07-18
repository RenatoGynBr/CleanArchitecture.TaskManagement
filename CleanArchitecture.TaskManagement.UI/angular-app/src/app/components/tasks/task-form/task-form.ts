import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { CreateTask } from '../../../models/task.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.scss',
})
export class TaskFormComponent {
  
  @Output() taskSubmitted = new EventEmitter<CreateTask>();
  taskForm!: FormGroup;

  ngOnInit() {
  this.taskForm = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(3)]],
    description: [''],
    dueDate: [null]
  });
  }
  constructor(private fb: FormBuilder) {}
  onSubmit() {
    if (this.taskForm.invalid) return;
    
    this.taskSubmitted.emit(this.taskForm.value as CreateTask);
    this.taskForm.reset();
  }
}