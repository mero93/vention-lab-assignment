import { LucideIcon } from 'lucide-react';

export const StatusDisplay = ({
  icon: Icon,
  title,
  message,
  variant = 'default',
}: {
  icon: LucideIcon;
  title: string;
  message?: string;
  variant?: 'default' | 'error';
}) => (
  <div className="flex min-h-100 w-full flex-col items-center justify-center rounded-xl border-2 border-dashed border-muted p-12 text-center">
    <div className={`rounded-full p-4 ${variant === 'error' ? 'bg-red-50' : 'bg-muted/50'}`}>
      <Icon
        className={`h-10 w-10 ${variant === 'error' ? 'text-red-500' : 'text-muted-foreground'}`}
      />
    </div>
    <h2 className="mt-5 text-xl font-semibold tracking-tight">{title}</h2>
    {message && <p className="mt-2 text-sm text-muted-foreground max-w-xs mx-auto">{message}</p>}
  </div>
);
