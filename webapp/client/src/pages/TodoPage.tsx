import React, { useState, useEffect } from 'react';
import { Link } from 'wouter';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Checkbox } from '@/components/ui/checkbox';
import { Plus, Edit, Trash2, MessageSquare, FolderOpen } from 'lucide-react';
import { apiService } from '@/services/api';
import { Todo } from '@/types';
import { useToast } from '@/hooks/use-toast';
import CommentsSection from '@/components/CommentsSection';

export default function TodoPage() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedTodo, setSelectedTodo] = useState<Todo | null>(null);
  const { toast } = useToast();

  useEffect(() => {
    loadTodos();
  }, []);

  const loadTodos = async () => {
    try {
      const data = await apiService.getTodos();
      setTodos(data);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to load todos",
        variant: "destructive",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleToggleComplete = async (id: number, isCompleted: boolean) => {
    try {
      await apiService.updateTodo(id, { isCompleted });
      loadTodos();
      toast({
        title: "Success",
        description: `Todo ${isCompleted ? 'completed' : 'marked as incomplete'}`,
      });
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to update todo",
        variant: "destructive",
      });
    }
  };

  const handleDeleteTodo = async (id: number) => {
    if (!confirm('Are you sure you want to delete this todo?')) return;
    try {
      await apiService.deleteTodo(id);
      loadTodos();
      toast({
        title: "Success",
        description: "Todo deleted successfully",
      });
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete todo",
        variant: "destructive",
      });
    }
  };

  if (loading) {
    return (
      <div className="container mx-auto p-6">
        <div className="flex items-center justify-center h-64">
          <div className="text-lg">Loading todos...</div>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto p-6">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-3xl font-bold">My Tasks</h1>
          <p className="text-muted-foreground mt-1">
            Manage your tasks and collaborate with comments
          </p>
        </div>
        <div className="flex gap-3">
          <Link href="/projects">
            <Button variant="outline">
              <FolderOpen className="mr-2 h-4 w-4" />
              Projects
            </Button>
          </Link>
          <Link href="/create">
            <Button>
              <Plus className="mr-2 h-4 w-4" />
              New Task
            </Button>
          </Link>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Todo List */}
        <div>
          <h2 className="text-xl font-semibold mb-4">Tasks</h2>
          <div className="space-y-3">
            {todos.length === 0 ? (
              <Card>
                <CardContent className="text-center py-8">
                  <p className="text-muted-foreground">No todos yet. Create your first task!</p>
                </CardContent>
              </Card>
            ) : (
              todos.map((todo) => (
                <Card key={todo.id} className="hover:shadow-md transition-shadow">
                  <CardContent className="p-4">
                    <div className="flex items-start justify-between">
                      <div className="flex items-start space-x-3 flex-1">
                        <Checkbox
                          checked={todo.isCompleted}
                          onCheckedChange={(checked) => 
                            handleToggleComplete(todo.id, checked as boolean)
                          }
                        />
                        <div className="flex-1">
                          <h3 className={`font-medium ${todo.isCompleted ? 'line-through text-muted-foreground' : ''}`}>
                            {todo.title}
                          </h3>
                          {todo.description && (
                            <p className={`text-sm text-muted-foreground ${todo.isCompleted ? 'line-through' : ''}`}>
                              {todo.description}
                            </p>
                          )}
                          <div className="flex items-center gap-2 mt-2">
                            <Badge variant={todo.isCompleted ? "secondary" : "default"}>
                              {todo.isCompleted ? 'Completed' : 'Pending'}
                            </Badge>
                            <span className="text-xs text-muted-foreground">
                              {new Date(todo.createdAt).toLocaleDateString()}
                            </span>
                          </div>
                        </div>
                      </div>
                      <div className="flex items-center gap-2">
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => setSelectedTodo(todo)}
                        >
                          <MessageSquare className="h-4 w-4" />
                        </Button>
                        <Link href={`/edit/${todo.id}`}>
                          <Button variant="ghost" size="sm">
                            <Edit className="h-4 w-4" />
                          </Button>
                        </Link>
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => handleDeleteTodo(todo.id)}
                        >
                          <Trash2 className="h-4 w-4" />
                        </Button>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              ))
            )}
          </div>
        </div>

        {/* Comments Section */}
        <div>
          <h2 className="text-xl font-semibold mb-4">Comments</h2>
          {selectedTodo ? (
            <div>
              <div className="mb-4 p-3 bg-muted rounded-lg">
                <h3 className="font-medium">{selectedTodo.title}</h3>
                {selectedTodo.description && (
                  <p className="text-sm text-muted-foreground mt-1">
                    {selectedTodo.description}
                  </p>
                )}
              </div>
              <CommentsSection todoItemId={selectedTodo.id} />
            </div>
          ) : (
            <Card>
              <CardContent className="text-center py-8">
                <MessageSquare className="h-12 w-12 mx-auto text-muted-foreground mb-4" />
                <p className="text-muted-foreground">
                  Select a task to view and add comments
                </p>
              </CardContent>
            </Card>
          )}
        </div>
      </div>
    </div>
  );
}
