import { LoginCredentials, RegisterData, AuthResponse, Todo, CreateTodoData, UpdateTodoData, Project, CreateProjectData, UpdateProjectData, Comment, CreateCommentData, UpdateCommentData } from '../types';

const API_BASE = '/api/v1';

class ApiService {
  private getAuthHeaders(endpoint: string): Record<string, string> {
    const token = localStorage.getItem('token');
    if (!token) return {};
    if (endpoint === '/auth/login' || endpoint === '/auth/register') return {};
    return { Authorization: `Bearer ${token}` };
  }

  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const url = `${API_BASE}${endpoint}`;
    const config: RequestInit = {
      headers: {
        'Content-Type': 'application/json',
        ...this.getAuthHeaders(endpoint),
        ...options.headers,
      },
      ...options,
    };

    const response = await fetch(url, config);
    
    if (!response.ok) {
      let errorText = await response.text();
      let errorData;
      try {
        errorData = JSON.parse(errorText);
      } catch {
        errorData = { message: errorText };
      }
      const errorMsg = errorData.message || errorText || `HTTP error! status: ${response.status}`;
      // Debug log for status and error
      console.error('API error:', response.status, errorMsg, errorData);
      throw new Error(errorMsg);
    }

    if (response.status === 204) {
      return {} as T;
    }

    const text = await response.text();
    if (!text) {
      return {} as T;
    }
    return JSON.parse(text);
  }

  // Auth methods
  async login(credentials: LoginCredentials): Promise<AuthResponse> {
    return this.request<AuthResponse>('/auth/login', {
      method: 'POST',
      body: JSON.stringify(credentials),
    });
  }

  async register(userData: RegisterData): Promise<void> {
    return this.request<void>('/auth/register', {
      method: 'POST',
      body: JSON.stringify(userData),
    });
  }

  // Todo methods
  async getTodos(): Promise<Todo[]> {
    return this.request<Todo[]>('/todoitems');
  }

  async getTodo(id: number): Promise<Todo> {
    return this.request<Todo>(`/todoitems/${id}`);
  }

  async createTodo(todoData: CreateTodoData): Promise<Todo> {
    return this.request<Todo>('/todoitems', {
      method: 'POST',
      body: JSON.stringify(todoData),
    });
  }

  async updateTodo(id: number, todoData: UpdateTodoData): Promise<Todo> {
    return this.request<Todo>(`/todoitems/${id}`, {
      method: 'PUT',
      body: JSON.stringify(todoData),
    });
  }

  async deleteTodo(id: number): Promise<void> {
    return this.request<void>(`/todoitems/${id}`, {
      method: 'DELETE',
    });
  }

  // Project methods
  async getProjects(): Promise<Project[]> {
    return this.request<Project[]>('/projects');
  }

  async getProject(id: number): Promise<Project> {
    return this.request<Project>(`/projects/${id}`);
  }

  async createProject(projectData: CreateProjectData): Promise<Project> {
    return this.request<Project>('/projects', {
      method: 'POST',
      body: JSON.stringify(projectData),
    });
  }

  async updateProject(id: number, projectData: UpdateProjectData): Promise<Project> {
    return this.request<Project>(`/projects/${id}`, {
      method: 'PUT',
      body: JSON.stringify(projectData),
    });
  }

  async deleteProject(id: number): Promise<void> {
    return this.request<void>(`/projects/${id}`, {
      method: 'DELETE',
    });
  }

  // Comment methods
  async getComments(): Promise<Comment[]> {
    return this.request<Comment[]>('/comments');
  }

  async getCommentsByTodoItem(todoItemId: number): Promise<Comment[]> {
    return this.request<Comment[]>(`/comments/todo/${todoItemId}`);
  }

  async createComment(commentData: CreateCommentData): Promise<Comment> {
    return this.request<Comment>('/comments', {
      method: 'POST',
      body: JSON.stringify(commentData),
    });
  }

  async updateComment(id: number, commentData: UpdateCommentData): Promise<Comment> {
    return this.request<Comment>(`/comments/${id}`, {
      method: 'PUT',
      body: JSON.stringify(commentData),
    });
  }

  async deleteComment(id: number): Promise<void> {
    return this.request<void>(`/comments/${id}`, {
      method: 'DELETE',
    });
  }
}

export const apiService = new ApiService();
