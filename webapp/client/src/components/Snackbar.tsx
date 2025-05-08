import React from 'react';
import { CheckCircle, AlertCircle, X } from 'lucide-react';

interface SnackbarProps {
  message: string;
  type: 'success' | 'error';
  isVisible: boolean;
  onClose: () => void;
}

const Snackbar: React.FC<SnackbarProps> = ({ message, type, isVisible, onClose }) => {
  if (!isVisible) return null;

  const Icon = type === 'success' ? CheckCircle : AlertCircle;
  const bgColor = type === 'success' ? 'border-success' : 'border-destructive';
  const iconColor = type === 'success' ? 'text-success' : 'text-destructive';

  return (
    <div className={`fixed bottom-4 left-4 right-4 sm:left-auto sm:right-4 sm:w-96 z-50 transform transition-transform duration-300 ${isVisible ? 'translate-y-0' : 'translate-y-full'}`}>
      <div className={`bg-surface rounded-lg material-shadow-lg border-l-4 ${bgColor} p-4 fade-in`}>
        <div className="flex items-center">
          <Icon className={`mr-3 ${iconColor}`} size={20} />
          <div className="flex-1">
            <p className="font-medium text-text-primary">{message}</p>
          </div>
          <button
            onClick={onClose}
            className="text-text-secondary hover:text-text-primary ml-4"
          >
            <X size={16} />
          </button>
        </div>
      </div>
    </div>
  );
};

export default Snackbar;
