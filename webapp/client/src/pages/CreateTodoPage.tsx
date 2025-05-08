import React, { useState, useEffect } from 'react';
import { useLocation } from 'wouter';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { ArrowLeft, Plus } from 'lucide-react';
import { apiService } from '../services/api';
import { CreateTodoData, Project } from '../types';
import { useToast } from '@/hooks/use-toast';
import { useAuth } from '@/contexts/AuthContext';

const CreateTodoPage: React.FC = () => {
  const [formData, setFormData] = useState<CreateTodoData>({
    title: '',
    description: '',
    userId: '',
  });
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [, setLocation] = useLocation();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const { user } = useAuth();

  console.log('CreateTodoPage - User context:', user);
  console.log('CreateTodoPage - User authenticated:', !!user);
  console.log('CreateTodoPage - User email:', user?.email);
  console.log('CreateTodoPage - Form data:', formData);

  useEffect(() => {
    loadProjects();
  }, []);

  useEffect(() => {
    if (user) {
      setFormData(prev => ({ ...prev, userId: user.email }));
    }
  }, [user]);

  const loadProjects = async () => {
    try {
      const data = await apiService.getProjects();
      setProjects(data);
      // Set the first project as default if available
      if (data.length > 0 && !formData.projectId) {
        setFormData(prev => ({ ...prev, projectId: data[0].id }));
      }
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load projects",
        variant: "destructive",
      });
    } finally {
      setLoading(false);
    }
  };

  const createMutation = useMutation({
    mutationFn: (todoData: CreateTodoData) => apiService.createTodo(todoData),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['/api/todoitems'] });
      toast({
        title: "Success",
        description: "Todo created successfully!",
      });
      setLocation('/todos');
    },
    onError: (error) => {
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "Failed to create todo",
        variant: "destructive",
      });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.title.trim()) {
      toast({
        title: "Error",
        description: "Title is required",
        variant: "destructive",
      });
      return;
    }
    if (!formData.projectId) {
      toast({
        title: "Error",
        description: "Please select a project",
        variant: "destructive",
      });
      return;
    }
    if (!user) {
      toast({
        title: "Error",
        description: "You must be logged in to create a todo",
        variant: "destructive",
      });
      return;
    }
    
    const todoData: CreateTodoData = {
      ...formData,
      userId: user.email
    };
    createMutation.mutate(todoData);
  };

  const handleInputChange = (field: keyof CreateTodoData, value: string | number) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-muted">
        <div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="flex items-center justify-center h-64">
            <div className="text-lg">Loading projects...</div>
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
            <h1 className="text-3xl font-semibold text-foreground mb-2">Create New Todo</h1>
            <p className="text-muted-foreground">Add a new task to your todo list</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="relative">
              <input
                type="text"
                id="todoTitle"
                value={formData.title}
                onChange={(e) => handleInputChange('title', e.target.value)}
                className="w-full px-3 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent peer"
                placeholder=" "
                required
              />
              <label
                htmlFor="todoTitle"
                className="floating-label absolute left-3 top-3 text-muted-foreground pointer-events-none peer-focus:text-primary peer-focus:scale-75 peer-focus:-translate-y-5 peer-[:not(:placeholder-shown)]:scale-75 peer-[:not(:placeholder-shown)]:-translate-y-5 transition-all duration-200"
              >
                Todo Title *
              </label>
            </div>

            <div className="relative">
              <textarea
                id="todoDescription"
                rows={4}
                value={formData.description}
                onChange={(e) => handleInputChange('description', e.target.value)}
                className="w-full px-3 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent peer resize-none"
                placeholder=" "
              />
              <label
                htmlFor="todoDescription"
                className="floating-label absolute left-3 top-3 text-muted-foreground pointer-events-none peer-focus:text-primary peer-focus:scale-75 peer-focus:-translate-y-5 peer-[:not(:placeholder-shown)]:scale-75 peer-[:not(:placeholder-shown)]:-translate-y-5 transition-all duration-200"
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

            {/* Form Actions */}
            <div className="flex flex-col sm:flex-row justify-end space-y-4 sm:space-y-0 sm:space-x-4 pt-6 border-t border-gray-200">
              <button
                type="button"
                onClick={() => setLocation('/todos')}
                className="material-ripple bg-gray-100 text-text-primary px-6 py-3 rounded-md hover:bg-gray-200 focus:outline-none focus:ring-2 focus:ring-gray-300 font-medium transition-colors duration-200"
              >
                Cancel
              </button>
              <button
                type="submit"
                disabled={createMutation.isPending}
                className="material-ripple bg-primary text-white px-6 py-3 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-primary focus:ring-offset-2 font-medium transition-colors duration-200 flex items-center disabled:opacity-50"
              >
                <Plus className="mr-2" size={16} />
                {createMutation.isPending ? 'Creating...' : 'Create Todo'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default CreateTodoPage;
