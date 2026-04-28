'use client';
import React, { useEffect, useRef, useState } from 'react';
import { useFormContext } from 'react-hook-form';

import { Button } from '@/components/ui/button';
import { cn } from '@/lib/utils';
import { Input } from './ui/input';
import { Eye, EyeClosed } from 'lucide-react';

const CUSTOM_INPUT_DEBOUNCE_TIMER = 300;

export type InputProperties = {
  name: string;
  label: string;
  type: 'text' | 'password' | 'email' | 'number';
  placeholder?: string;
  classes?: string;
  dependencies?: string[];
  readonly?: boolean;
  allowedPattern?: RegExp;
};

export const CustomInput = ({
  name,
  label,
  type,
  placeholder,
  classes,
  dependencies,
  readonly,
  allowedPattern,
}: InputProperties) => {
  const { register, formState, getFieldState, watch, trigger } = useFormContext();
  const isPassword = type === 'password';
  const [visibility, setVisibility] = useState(false);

  const { error, isTouched, isDirty } = getFieldState(name, formState);

  const errorMessage = error?.message ?? '';

  const value = watch(name);

  const isEmpty = () => value == undefined || value == '';

  const hasInteracted = isTouched || isDirty;
  const hasError = Boolean(error) && hasInteracted;

  const [initialBlur, setInitialBlur] = useState(false);

  const shouldShowError = Boolean(error) && (initialBlur || !isEmpty());

  const {
    onChange: rhfOnChange,
    onBlur: rhfOnBlur,
    ...restRegister
  } = register(name, { valueAsNumber: type === 'number' });

  const debounceReference = useRef<NodeJS.Timeout | undefined>(undefined);

  const handleOnChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    let rawValue = event.target.value;

    if (allowedPattern) {
      rawValue = rawValue.replace(allowedPattern, '');
      event.target.value = rawValue;
    }

    rhfOnChange(event);

    if (debounceReference.current) clearTimeout(debounceReference.current);

    debounceReference.current = setTimeout(() => {
      if (!initialBlur) {
        setInitialBlur(true);
      }

      trigger(name);
      dependencies?.forEach((dependency) => {
        if (isDirty) trigger(dependency);
      });
    }, CUSTOM_INPUT_DEBOUNCE_TIMER);
  };

  const handleBlur = (event: React.FocusEvent<HTMLInputElement>) => {
    rhfOnBlur(event);

    setInitialBlur(true);
  };

  useEffect(() => {
    return () => {
      if (debounceReference.current) clearTimeout(debounceReference.current);
    };
  }, []);

  return (
    <div
      className={cn(
        'flex w-full flex-col gap-2',
        (readonly ?? false) && 'opacity-70 pointer-events-none',
        classes,
      )}
    >
      <label className="text-sm font-semibold tracking-tight text-foreground/80" htmlFor={name}>
        {label}
      </label>

      <div className="relative flex w-full items-center">
        <Input
          {...restRegister}
          id={name}
          type={visibility ? 'text' : type}
          placeholder={placeholder}
          className={cn(
            'w-full transition-all duration-200',
            hasError
              ? 'border-destructive text-destructive focus-visible:ring-destructive/20'
              : 'border-input focus-visible:border-info/50 focus-visible:ring-info/20',
            'placeholder:text-muted-foreground/50',
          )}
          onChange={handleOnChange}
          onBlur={handleBlur}
          readOnly={readonly}
          tabIndex={(readonly ?? false) ? -1 : 0}
        />

        {isPassword && (
          <Button
            type="button"
            variant="ghost"
            size="icon"
            onClick={() => setVisibility(!visibility)}
            className={cn(
              'absolute right-2 h-8 w-8 text-muted-foreground hover:bg-transparent hover:text-foreground',
              hasError && 'text-destructive hover:text-destructive',
            )}
            tabIndex={-1}
          >
            {visibility ? <Eye className="size-4" /> : <EyeClosed className="size-4" />}
          </Button>
        )}
      </div>

      <div className="min-h-5">
        {shouldShowError && (
          <p className="text-[12px] font-medium text-destructive animate-in fade-in slide-in-from-top-1">
            {isEmpty() ? `${label} is required` : errorMessage}
          </p>
        )}
      </div>
    </div>
  );
};
