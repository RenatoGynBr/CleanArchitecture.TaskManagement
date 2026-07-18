export interface Task {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: Date;
  dueDate?: Date;
}

export interface CreateTask {
  title: string;
  description: string;
  dueDate?: Date;
}

export interface UpdateTask {
  title?: string;
  description?: string;
  isCompleted?: boolean;
  dueDate?: Date;
}