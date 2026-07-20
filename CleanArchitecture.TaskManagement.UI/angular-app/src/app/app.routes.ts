import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { 
    path: '', 
    redirectTo: '/tasks', 
    pathMatch: 'full' 
  },
  { 
    path: 'login', 
    loadComponent: () => 
      import('./components/auth/login/login')
        .then(m => m.LoginComponent) 
  },
  {
    path: 'register', 
    loadComponent: () => 
      import('./components/auth/register/register')
        .then(m => m.RegisterComponent) 
  },
  
  { 
    path: 'tasks', 
    loadComponent: () => 
      import('./components/tasks/task-list/task-list')
        .then(m => m.TaskListComponent),
    canActivate: [authGuard]  // 🔐 Protected route
  }
];
