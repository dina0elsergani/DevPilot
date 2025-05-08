import React, { useState } from 'react';
import { Link, useLocation } from 'wouter';
import { UserPlus, AlertCircle, CheckCircle } from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';
import { RegisterData } from '../types';

const RegisterPage: React.FC = () => {
  const [formData, setFormData] = useState<RegisterData & { confirmPassword: string }>({
    email: '',
    password: '',
    confirmPassword: '',
  });
  const [formErrors, setFormErrors] = useState<string[]>([]);
  const [success, setSuccess] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const { register } = useAuth();
  const [, setLocation] = useLocation();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setFormErrors([]);
    setSuccess(false);

    const localErrors: string[] = [];
    if (formData.password !== formData.confirmPassword) {
      localErrors.push('Passwords do not match');
    }
    if (formData.password.length < 6) {
      localErrors.push('Password must be at least 6 characters long');
    }
    if (!/[A-Z]/.test(formData.password)) {
      localErrors.push('Password must contain at least one uppercase letter');
    }
    if (!/[0-9]/.test(formData.password)) {
      localErrors.push('Password must contain at least one digit');
    }
    if (!/[^a-zA-Z0-9]/.test(formData.password)) {
      localErrors.push('Password must contain at least one special character');
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      localErrors.push('Please enter a valid email address');
    }
    if (localErrors.length > 0) {
      setFormErrors(localErrors);
      return;
    }

    setIsLoading(true);

    try {
      await register({
        email: formData.email,
        password: formData.password,
      });
      setSuccess(true);
      setTimeout(() => {
        setLocation('/login');
      }, 2000);
    } catch (error: any) {
      // Try to extract backend errors
      let errors: string[] = [];
      if (error && error.message) {
        try {
          const parsed = JSON.parse(error.message);
          if (parsed.errors && Array.isArray(parsed.errors)) {
            errors = parsed.errors;
          } else if (parsed.message) {
            errors = [parsed.message];
          }
        } catch {
          errors = [error.message];
        }
      }
      setFormErrors(errors);
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (field: keyof typeof formData, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    setFormErrors([]);
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-secondary to-pink-700">
      <div className="bg-card rounded-lg shadow-lg p-8 w-full max-w-md mx-4 animate-fadeIn">
        <div className="text-center mb-8">
          <UserPlus className="text-secondary text-5xl mb-4 mx-auto" />
          <h2 className="text-2xl font-semibold text-foreground mb-2">Create Account</h2>
          <p className="text-muted-foreground">Join us to start organizing your tasks</p>
        </div>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="relative">
            <input
              type="email"
              id="email"
              value={formData.email}
              onChange={(e) => handleInputChange('email', e.target.value)}
              className="w-full px-3 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-secondary focus:border-transparent peer"
              placeholder=" "
              required
            />
            <label
              htmlFor="email"
              className="floating-label absolute left-3 top-3 text-text-secondary pointer-events-none peer-focus:text-secondary peer-focus:scale-75 peer-focus:-translate-y-5 peer-[:not(:placeholder-shown)]:scale-75 peer-[:not(:placeholder-shown)]:-translate-y-5 transition-all duration-200"
            >
              Email
            </label>
          </div>

          <div className="relative">
            <input
              type="password"
              id="password"
              value={formData.password}
              onChange={(e) => handleInputChange('password', e.target.value)}
              className="w-full px-3 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-secondary focus:border-transparent peer"
              placeholder=" "
              required
            />
            <label
              htmlFor="password"
              className="floating-label absolute left-3 top-3 text-text-secondary pointer-events-none peer-focus:text-secondary peer-focus:scale-75 peer-focus:-translate-y-5 peer-[:not(:placeholder-shown)]:scale-75 peer-[:not(:placeholder-shown)]:-translate-y-5 transition-all duration-200"
            >
              Password
            </label>
            <div className="text-xs text-muted-foreground mt-1">
              Password must be at least 6 characters, contain an uppercase letter, a digit, and a special character.
            </div>
          </div>

          <div className="relative">
            <input
              type="password"
              id="confirmPassword"
              value={formData.confirmPassword}
              onChange={(e) => handleInputChange('confirmPassword', e.target.value)}
              className="w-full px-3 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-secondary focus:border-transparent peer"
              placeholder=" "
              required
            />
            <label
              htmlFor="confirmPassword"
              className="floating-label absolute left-3 top-3 text-text-secondary pointer-events-none peer-focus:text-secondary peer-focus:scale-75 peer-focus:-translate-y-5 peer-[:not(:placeholder-shown)]:scale-75 peer-[:not(:placeholder-shown)]:-translate-y-5 transition-all duration-200"
            >
              Confirm Password
            </label>
          </div>

          {success && (
            <div className="bg-green-50 border border-green-200 text-success px-4 py-3 rounded-md">
              <div className="flex items-center">
                <CheckCircle className="mr-2" size={16} />
                <span>Account created successfully! Please sign in.</span>
              </div>
            </div>
          )}

          {(formErrors.length > 0) && (
            <div className="bg-red-50 border border-red-200 text-destructive px-4 py-3 rounded-md">
              <div className="flex items-center mb-1">
                <AlertCircle className="mr-2" size={16} />
                <span>Registration failed:</span>
              </div>
              {formErrors.length > 0 && (
                <ul className="list-disc list-inside ml-6">
                  {formErrors.map((msg, idx) => (
                    <li key={idx}>{msg}</li>
                  ))}
                </ul>
              )}
            </div>
          )}

          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-secondary text-secondary-foreground py-3 px-4 rounded-md hover:bg-secondary/90 focus:outline-none focus:ring-2 focus:ring-secondary focus:ring-offset-2 font-medium transition-colors duration-200 disabled:opacity-50"
          >
            {isLoading ? 'Creating Account...' : 'Create Account'}
          </button>
        </form>

        <div className="mt-6 text-center">
          <p className="text-muted-foreground">
            Already have an account?{' '}
            <Link href="/login" className="text-secondary hover:text-secondary/80 font-medium">
              Sign in
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default RegisterPage;
