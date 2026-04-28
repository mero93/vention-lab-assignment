import Image from 'next/image';
import { Product } from '@/types/product';
import { STORAGE_PATHS } from '@/lib/constants';
import ProductDetailsDialog from './ProductDetailsDialog';
import { useState } from 'react';

export default function ProductCard({ product }: Readonly<{ product: Product }>) {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <>
      <div
        className="group cursor-pointer relative aspect-square w-full overflow-hidden rounded-xl border bg-muted shadow-sm transition-all hover:shadow-xl"
        onClick={() => setIsOpen(true)}
      >
        <Image
          src={`${STORAGE_PATHS.UPLOADS}/${product.image}`}
          alt={product.title}
          fill
          unoptimized
          className="object-cover transition-transform duration-500 group-hover:scale-110"
          sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
        />
        <div className="absolute inset-0 bg-linear-to-t from-black/80 via-black/40 to-transparent opacity-0 transition-opacity duration-300 group-hover:opacity-100" />
        <div className="absolute inset-0 flex flex-col justify-end p-6 opacity-0 transition-all duration-300 translate-y-4 group-hover:opacity-100 group-hover:translate-y-0">
          <h3 className="text-lg font-bold text-white drop-shadow-md">{product.title}</h3>
          {product.description && (
            <p className="mt-1 text-sm text-gray-200 line-clamp-2 drop-shadow-sm">
              {product.description}
            </p>
          )}
        </div>
      </div>
      <ProductDetailsDialog product={product} open={isOpen} setOpen={setIsOpen} />
    </>
  );
}
