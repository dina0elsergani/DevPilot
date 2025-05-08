export interface User {
  id: number;
  email: string;
  createdAt: string;
}

export interface Todo {
  id: number;
  title: string;
  description: string | null;
  isCompleted: boolean;
  projectId: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTodoData {
  title: string;
  description?: string;
  projectId?: number;
  userId: string;
}

export interface UpdateTodoData {
  title?: string;
  description?: string;
  isCompleted?: boolean;
  projectId?: number;
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RegisterData {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
}

export interface TodoStats {
  total: number;
  completed: number;
  pending: number;
}

export interface Project {
  id: number;
  name: string;
  description?: string;
  createdAt: string;
  todoItems: Todo[];
}

export interface CreateProjectData {
  name: string;
  description?: string;
  userId: string;
}

export interface UpdateProjectData {
  name: string;
  description?: string;
}

export interface Comment {
  id: number;
  content: string;
  createdAt: string;
  todoItemId: number;
}

export interface CreateCommentData {
  content: string;
  todoItemId: number;
}

export interface UpdateCommentData {
  content: string;
}
