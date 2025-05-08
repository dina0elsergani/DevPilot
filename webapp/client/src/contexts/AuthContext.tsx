import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User, LoginCredentials, RegisterData } from '../types';
import { apiService } from '../services/api';
import { useToast } from '@/hooks/use-toast';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (credentials: LoginCredentials) => Promise<void>;
  register: (userData: RegisterData) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const { toast } = useToast();

  const isAuthenticated = !!user;

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      // Decode token to get user info (use correct claim names)
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        console.log('JWT payload:', payload);
        setUser({
          id: payload.sub,
          email: payload.unique_name,
          createdAt: new Date().toISOString(),
        });
        console.log('User set from token:', { id: payload.sub, email: payload.unique_name });
      } catch (error) {
        console.error('Error decoding JWT token:', error);
        localStorage.removeItem('token');
      }
    }
    setIsLoading(false);
  }, []);

  const login = async (credentials: LoginCredentials) => {
    try {
      console.log('Login attempt with credentials:', credentials);
      const response = await apiService.login(credentials);
      console.log('Login response:', response);
      localStorage.setItem('token', response.token);
      
      // Decode token to get user info
      const payload = JSON.parse(atob(response.token.split('.')[1]));
      console.log('Login JWT payload:', payload);
      setUser({
        id: payload.sub,
        email: payload.unique_name,
        createdAt: new Date().toISOString(),
      });
      console.log('User set after login:', { id: payload.sub, email: payload.unique_name });

      toast({
        title: "Success",
        description: "Login successful!",
      });
    } catch (error) {
      console.error('Login error:', error);
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "Login failed",
        variant: "destructive",
      });
      throw error;
    }
  };

  const register = async (userData: RegisterData) => {
    try {
      await apiService.register(userData);
      toast({
        title: "Success",
        description: "Account created successfully! Please sign in.",
      });
    } catch (error) {
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "Registration failed",
        variant: "destructive",
      });
      throw error;
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
    toast({
      title: "Success",
      description: "Logged out successfully",
    });
  };

  const value: AuthContextType = {
    user,
    isAuthenticated,
    isLoading,
    login,
    register,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
