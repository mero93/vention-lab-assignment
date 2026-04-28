'use client';

import { Dialog, DialogContent, DialogTitle } from '@/components/ui/dialog';
import Image from 'next/image';
import { Product } from '@/types/product';
import { STORAGE_PATHS } from '@/lib/constants';
import * as VisuallyHidden from '@radix-ui/react-visually-hidden';

interface ProductDetailsDialogProps {
  product: Product | null;
  open: boolean;
  setOpen: (open: boolean) => void;
}

export default function ProductDetailsDialog({
  product,
  open,
  setOpen,
}: Readonly<ProductDetailsDialogProps>) {
  if (!product) return null;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="max-w-200! w-full max-h-200! h-full p-0 border-none overflow-hidden bg-black flex items-center justify-center">
        <VisuallyHidden.Root>
          <DialogTitle>{product.title}</DialogTitle>
        </VisuallyHidden.Root>

        <div className="relative w-full h-full">
          <Image
            src={`${STORAGE_PATHS.UPLOADS}/${product.image}`}
            alt={product.title}
            fill
            unoptimized
            className="object-contain"
            priority
          />

          <div className="absolute inset-x-0 bottom-0 bg-linear-to-t from-black/95 via-black/60 to-transparent px-8 pb-10 pt-32">
            <h2 className="text-3xl font-bold text-white drop-shadow-md">{product.title}</h2>
            {product.description && (
              <p className="mt-3 text-lg text-gray-200 leading-relaxed drop-shadow-sm">
                {product.description}
              </p>
            )}
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
