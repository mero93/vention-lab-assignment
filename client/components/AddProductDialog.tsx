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

  const [validating, setValidating] = useState(false);

  const {
    handleSubmit,
    formState: { isSubmitting, isValid },
  } = methods;

  const onSubmit = async (data: AddProduct) => {
    try {
      setValidating(true);
      console.log('Form Data:', data);

      const formData = new FormData();
      formData.append('title', data.title);
      if (data.description) formData.append('description', data.description);
      if (data.image instanceof File) {
        formData.append('image', data.image);
      }

      // await productService.addProduct(formData)

      setOpen(false);
    } catch (error) {
      console.error('Failed to add product', error);
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
              <Button type="button" variant="outline" onClick={() => setOpen(false)} disabled={validating}>
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
