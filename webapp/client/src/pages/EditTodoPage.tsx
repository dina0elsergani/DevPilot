import React, { useState, useEffect } from 'react';
import { useLocation, useParams } from 'wouter';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ArrowLeft, Save, Trash2 } from 'lucide-react';
import { apiService } from '../services/api';
import { UpdateTodoData, Project } from '../types';
import { useToast } from '@/hooks/use-toast';

const EditTodoPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const todoId = parseInt(id || '0');
  const [, setLocation] = useLocation();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [formData, setFormData] = useState<UpdateTodoData>({
    title: '',
    description: '',
    isCompleted: false,
  });
  const [projects, setProjects] = useState<Project[]>([]);

  const { data: todo, isLoading } = useQuery({
    queryKey: ['/api/todoitems', todoId],
    queryFn: () => apiService.getTodo(todoId),
    enabled: !!todoId,
  });

  useEffect(() => {
    loadProjects();
  }, []);

  const loadProjects = async () => {
    try {
      const data = await apiService.getProjects();
      setProjects(data);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load projects",
        variant: "destructive",
      });
    }
  };

  useEffect(() => {
    if (todo) {
      setFormData({
        title: todo.title,
        description: todo.description || '',
        isCompleted: todo.isCompleted,
        projectId: todo.projectId,
      });
    }
  }, [todo]);

  const updateMutation = useMutation({
    mutationFn: (todoData: UpdateTodoData) => apiService.updateTodo(todoId, todoData),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['/api/todoitems'] });
      queryClient.invalidateQueries({ queryKey: ['/api/todoitems', todoId] });
      toast({
        title: "Success",
        description: "Todo updated successfully!",
      });
      setLocation('/todos');
    },
    onError: (error) => {
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "Failed to update todo",
        variant: "destructive",
      });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: () => apiService.deleteTodo(todoId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['/api/todoitems'] });
      toast({
        title: "Success",
        description: "Todo deleted successfully!",
      });
      setLocation('/todos');
    },
    onError: (error) => {
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "Failed to delete todo",
        variant: "destructive",
      });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    // Clean and validate form data
    const cleanedData: UpdateTodoData = {
      title: formData.title?.trim() || null,
      description: formData.description?.trim() || null,
      isCompleted: formData.isCompleted,
      projectId: formData.projectId
    };
    
    // Validate required fields
    if (!cleanedData.title) {
      toast({
        title: "Error",
        description: "Title is required",
        variant: "destructive",
      });
      return;
    }
    
    if (!cleanedData.projectId) {
      toast({
        title: "Error",
        description: "Project is required",
        variant: "destructive",
      });
      return;
    }
    
    console.log('EditTodoPage - Form data:', cleanedData);
    updateMutation.mutate(cleanedData);
  };

  const handleDelete = () => {
    if (window.confirm('Are you sure you want to delete this todo?')) {
      deleteMutation.mutate();
    }
  };

  const handleInputChange = (field: keyof UpdateTodoData, value: string | boolean | number) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-muted">
        <div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="animate-pulse bg-gray-200 h-96 rounded-lg"></div>
        </div>
      </div>
    );
  }

  if (!todo) {
    return (
      <div className="min-h-screen bg-muted">
        <div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="text-center">
            <h1 className="text-2xl font-semibold text-foreground mb-2">Todo not found</h1>
            <button
              onClick={() => setLocation('/todos')}
              className="text-primary hover:text-blue-700"
            >
              Back to Todos
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-muted">
      <div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Back Button */}
        <div className="mb-6">
          <button
            onClick={() => setLocation('/todos')}
            className="flex items-center text-muted-foreground hover:text-foreground transition-colors duration-200"
          >
            <ArrowLeft className="mr-2" size={20} />
            Back to Todos
          </button>
        </div>

        <div className="bg-card rounded-lg shadow-sm p-8">
          <div className="mb-8">
            <h1 className="text-3xl font-semibold text-foreground mb-2">Edit Todo</h1>
            <p className="text-muted-foreground">Update your task details</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="relative">
              <input
                type="text"
                id="editTodoTitle"
                value={formData.title || ''}
                onChange={(e) => handleInputChange('title', e.target.value)}
                className="w-full px-3 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent peer"
                placeholder=" "
                required
              />
              <label
                htmlFor="editTodoTitle"
                className={`floating-label absolute left-3 pointer-events-none transition-all duration-200 ${
                  formData.title ? 'top-0 -translate-y-2 scale-75 text-primary' : 'top-3 text-muted-foreground'
                }`}
              >
                Todo Title *
              </label>
            </div>

            <div className="relative">
              <textarea
                id="editTodoDescription"
                rows={4}
                value={formData.description || ''}
                onChange={(e) => handleInputChange('description', e.target.value)}
                className="w-full px-3 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent peer resize-none"
                placeholder=" "
              />
              <label
                htmlFor="editTodoDescription"
                className={`floating-label absolute left-3 pointer-events-none transition-all duration-200 ${
                  formData.description ? 'top-0 -translate-y-2 scale-75 text-primary' : 'top-3 text-muted-foreground'
                }`}
              >
                Description
              </label>
            </div>

            {/* Project Selection */}
            <div className="relative">
              <select
                id="projectId"
                value={formData.projectId || ''}
                onChange={(e) => handleInputChange('projectId', parseInt(e.target.value))}
                className="w-full px-3 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent"
                required
              >
                <option value="">Select a project</option>
                {projects.map((project) => (
                  <option key={project.id} value={project.id}>
                    {project.name}
                  </option>
                ))}
              </select>
              <label
                htmlFor="projectId"
                className="absolute left-3 -top-2 bg-white px-2 text-sm text-muted-foreground"
              >
                Project *
              </label>
            </div>

            <div className="flex items-center space-x-3">
              <input
                type="checkbox"
                id="editTodoCompleted"
                checked={formData.isCompleted || false}
                onChange={(e) => handleInputChange('isCompleted', e.target.checked)}
                className="w-5 h-5 text-primary border-gray-300 rounded focus:ring-primary focus:ring-2"
              />
              <label htmlFor="editTodoCompleted" className="text-sm font-medium text-foreground">
                Mark as completed
              </label>
            </div>

            {/* Form Actions */}
            <div className="flex flex-col sm:flex-row justify-end space-y-4 sm:space-y-0 sm:space-x-4 pt-6 border-t border-gray-200">
              <button
                type="button"
                onClick={handleDelete}
                disabled={deleteMutation.isPending}
                className="material-ripple bg-red-600 text-white px-6 py-3 rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 font-medium transition-colors duration-200 flex items-center disabled:opacity-50"
              >
                <Trash2 className="mr-2" size={16} />
                {deleteMutation.isPending ? 'Deleting...' : 'Delete Todo'}
              </button>
              <button
                type="button"
                onClick={() => setLocation('/todos')}
                className="material-ripple bg-gray-100 text-text-primary px-6 py-3 rounded-md hover:bg-gray-200 focus:outline-none focus:ring-2 focus:ring-gray-300 font-medium transition-colors duration-200"
              >
                Cancel
              </button>
              <button
                type="submit"
                disabled={updateMutation.isPending}
                className="material-ripple bg-primary text-white px-6 py-3 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-primary focus:ring-offset-2 font-medium transition-colors duration-200 flex items-center disabled:opacity-50"
              >
                <Save className="mr-2" size={16} />
                {updateMutation.isPending ? 'Saving...' : 'Save Changes'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default EditTodoPage;
