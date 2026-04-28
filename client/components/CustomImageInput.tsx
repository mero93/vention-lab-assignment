import { ImageOff, Upload, XIcon } from 'lucide-react';
import Image from 'next/image';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useDropzone } from 'react-dropzone';
import { useFormContext } from 'react-hook-form';

import { cn } from '@/lib/utils';
import { Button } from './ui/button';

const CUSTOM_INPUT_DEBOUNCE_TIMER = 300;

type CSSValue =
  | { value: number; unit: 'px' }
  | { value: number; unit: 'rem' }
  | { value: number; unit: '%' }
  | { value: number; unit: 'vw' }
  | { value: number; unit: 'vh' };

type ImageInputProperties = {
  name: string;
  label: string;
  dependencies?: string[];
  classes?: string;
  readonly?: boolean;
  width?: CSSValue;
  height?: CSSValue;
};

export default function CustomImageInput({
  name,
  label,
  dependencies,
  classes,
  readonly,
  width,
  height,
}: Readonly<ImageInputProperties>) {
  const { register, formState, getFieldState, watch, trigger, setValue } = useFormContext();
  const { error, isTouched, isDirty } = getFieldState(name, formState);
  const [validating, setValidating] = useState(false);
  const inputRef = useRef<HTMLInputElement | null>(null);

  const value = watch(name);

  const isEmpty = () => value == undefined || value == '' || value.length === 0;

  const hasInteracted = isTouched || isDirty || true;
  const hasError = Boolean(error) && hasInteracted;

  const [initialBlur, setInitialBlur] = useState(false);

  const debounceReference = useRef<NodeJS.Timeout | undefined>(undefined);

  const { ref: rhfRef, onBlur: rhfOnBlur, ...restRegister } = register(name);

  const previewUrl = useMemo(() => {
    if (!error && value && !validating && (value instanceof File || value instanceof Blob)) {
      return URL.createObjectURL(value);
    }
    return null;
  }, [error, validating, value]);

  useEffect(() => {
    return () => {
      if (previewUrl) URL.revokeObjectURL(previewUrl);
    };
  }, [previewUrl]);

  const updateFileValue = useCallback(
    (file: File | undefined) => {
      setValue(name, file, {
        shouldValidate: true,
        shouldDirty: true,
        shouldTouch: true,
      });

      setValidating(true);
      if (debounceReference.current) clearTimeout(debounceReference.current);

      debounceReference.current = setTimeout(() => {
        if (!initialBlur) setInitialBlur(true);
        trigger(name);
        setValidating(false);
        dependencies?.forEach((dependency) => {
          if (isDirty) trigger(dependency);
        });
      }, CUSTOM_INPUT_DEBOUNCE_TIMER);
    },
    [name, setValue, trigger, dependencies, isDirty, initialBlur],
  );

  const handleOnChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const fileList = event.target.files;

    const file = fileList && fileList.length > 0 ? fileList[0] : undefined;

    updateFileValue(file);
  };

  const handleBlur = (event: React.FocusEvent<HTMLInputElement>) => {
    rhfOnBlur(event);

    setInitialBlur(true);
  };

  const handleOnClick = (event: React.MouseEvent) => {
    event.preventDefault();
    inputRef.current?.click();
  };

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop: (files) => {
      if (files.length > 0) updateFileValue(files[0]);
    },
    noClick: true,
    disabled: readonly,
    accept: { 'image/*': ['.jpeg', '.jpg', '.png', '.webp'] },
    multiple: false,
  });

  useEffect(() => {
    if (inputRef.current) {
      rhfRef(inputRef.current);
    }
  }, [rhfRef]);

  const cleanInput = (event: React.MouseEvent) => {
    event.stopPropagation();
    if (inputRef.current == undefined) return;

    inputRef.current.value = '';

    updateFileValue(undefined);
  };

  useEffect(() => {
    setValue(name, undefined);
    trigger();

    return () => {
      if (debounceReference.current) clearTimeout(debounceReference.current);
    };
  }, [name, setValue, trigger]);

  return (
    <div className={cn('flex w-full flex-col gap-2', classes)}>
      <label className="text-sm font-semibold text-foreground/80" htmlFor={name}>
        {label}
      </label>

      <div
        {...getRootProps()}
        className={cn(
          'relative flex flex-col items-center justify-center rounded-lg border-2 border-dashed transition-colors',
          'border-border bg-muted/20',
          'hover:border-info/50 hover:bg-info/5',
          isDragActive && 'border-info bg-info/10',
          !isEmpty() && 'border-solid border-border bg-card shadow-sm',
          readonly && 'opacity-50 pointer-events-none',
        )}
        style={{
          width: width ? `${width.value}${width.unit}` : '100%',
          minHeight: height ? `${height.value}${height.unit}` : '160px',
        }}
      >
        <input
          {...restRegister}
          {...getInputProps()}
          ref={inputRef}
          onChange={handleOnChange}
          onBlur={handleBlur}
          id={name}
          type="file"
          className="hidden"
          readOnly={readonly}
        />

        {isEmpty() ? (
          <div className="flex flex-col items-center gap-3 p-6 text-center">
            <Upload className="size-10 text-muted-foreground/40" strokeWidth={1.5} />
            <div className="flex flex-col gap-1">
              <p className="text-sm text-muted-foreground">
                Drag file here or{' '}
                <button
                  type="button"
                  className="font-bold text-info hover:underline"
                  onClick={handleOnClick}
                >
                  browse
                </button>
              </p>
              <p className="text-xs text-muted-foreground/50">PNG, JPG or WebP</p>
            </div>
            {hasError && initialBlur && (
              <p className="text-xs font-medium text-destructive mt-2">{label} is required</p>
            )}
          </div>
        ) : (
          <div className="flex w-full items-center gap-4 px-6 py-4">
            <div className="relative shrink-0">
              <div className="flex size-16 items-center justify-center overflow-hidden rounded-md border border-border bg-muted/50">
                {previewUrl ? (
                  <Image
                    src={previewUrl}
                    alt="Preview"
                    width={64}
                    height={64}
                    className="h-full w-full object-cover"
                  />
                ) : (
                  <ImageOff className="size-8 text-muted-foreground/30" />
                )}
              </div>
              <Button
                variant="destructive"
                size="icon"
                className="absolute -left-2 -top-2 size-6 rounded-full"
                onClick={cleanInput}
              >
                <XIcon className="size-3" />
              </Button>
            </div>

            <div className="flex flex-1 flex-col gap-1 overflow-hidden">
              <p className="truncate text-sm font-medium">{value.name}</p>
              <p className="text-[10px] font-mono text-muted-foreground uppercase">
                {formatSize(value.size)}
              </p>
              <Button
                type="button"
                variant="outline"
                size="sm"
                className="mt-1 w-fit h-7 text-xs border-info/20 text-info hover:bg-info/10"
                onClick={handleOnClick}
              >
                Change Image
              </Button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

const formatSize = (bytes: number) => (bytes / (1024 * 1024)).toFixed(2) + ' MB';
