'use client';

import { useForm, FormProvider } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import CustomImageInput from './CustomImageInput';
import { AddProduct, AddProductSchema } from '@/types/schemas/add-product-schema';
import { CustomInput } from './CustomInput';
import { useEffect, useState } from 'react';
import { createProduct, uploadImage } from '@/lib/api';
import { Product } from '@/types/product';
import { useRouter } from 'next/navigation';

type LoginDialogProperties = {
  open: boolean;
  setOpen: (bool: boolean) => void;
};

export default function AddProductDialog({ open, setOpen }: Readonly<LoginDialogProperties>) {
  const methods = useForm<AddProduct>({
    resolver: zodResolver(AddProductSchema),
    defaultValues: {
      title: undefined,
      description: undefined,
      image: undefined,
    },
  });

  const router = useRouter();

  const [validating, setValidating] = useState(false);

  const {
    handleSubmit,
    reset,
    resetField,
    formState: { isSubmitting, isValid },
  } = methods;

  const onSubmit = async (formData: AddProduct) => {
    try {
      setValidating(true);

      const uploadResult = await uploadImage(formData.image);

      if (uploadResult.error || !uploadResult.data) {
        resetField('image');
        return;
      }

      const product: Omit<Product, 'id'> = {
        title: formData.title,
        description: formData.description ?? undefined,
        image: uploadResult.data.fileName,
      };

      const { data, error } = await createProduct(product);

      if (error || !data) {
        reset();
        return;
      }

      router.refresh();
      setOpen(false);
    } catch {
      reset();
    } finally {
      setValidating(false);
    }
  };

  useEffect(() => console.log('validation changed', isValid), [isValid]);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="sm:max-w-125">
        <DialogHeader>
          <DialogTitle>Add New Media Product</DialogTitle>
        </DialogHeader>

        <FormProvider {...methods}>
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 py-4">
            <CustomImageInput name="image" label="Product Cover" />

            <CustomInput
              name="title"
              label="Title"
              placeholder="e.g. Inception Vinyl Edition"
              type="text"
            />

            <CustomInput
              name="description"
              label="Description"
              placeholder="Brief details about the item..."
              type="text"
            />

            <DialogFooter className="pt-4">
              <Button
                type="button"
                variant="outline"
                onClick={() => setOpen(false)}
                disabled={validating}
              >
                Cancel
              </Button>
              <Button type="submit" variant="info" disabled={!isValid || isSubmitting}>
                {validating ? 'Saving...' : 'Save Product'}
              </Button>
            </DialogFooter>
          </form>
        </FormProvider>
      </DialogContent>
    </Dialog>
  );
}
