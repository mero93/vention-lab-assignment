'use client';

import { usePathname, useRouter, useSearchParams } from 'next/navigation';
import { Product } from '@/types/product';
import { PaginatedResult } from '@/types/paginated-result';
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from '@/components/ui/pagination';
import ProductCard from './ProductCard';

export default function ProductLibrary({
  paginatedResult,
}: Readonly<{
  paginatedResult: PaginatedResult<Product>;
}>) {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const { items, totalPages, pageNumber } = paginatedResult;

  const createPageUrl = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());
    params.set('pageNumber', page.toString());
    return `${pathname}?${params.toString()}`;
  };

  const handlePageChange = (e: React.MouseEvent, page: number) => {
    e.preventDefault();
    if (page < 1 || page > totalPages) return;
    router.push(createPageUrl(page));
  };

  return (
    <div className="flex flex-col gap-8">
      <div className="flex justify-between items-center border-b py-4">
        <Pagination className="justify-end w-auto">
          <PaginationContent>
            <PaginationItem>
              <PaginationPrevious
                href={createPageUrl(pageNumber - 1)}
                onClick={(e) => handlePageChange(e, pageNumber - 1)}
                className={pageNumber <= 1 ? 'pointer-events-none opacity-50' : 'cursor-pointer'}
              />
            </PaginationItem>

            {Array.from({ length: totalPages }, (_, i) => i + 1).map((p) => (
              <PaginationItem key={p} className="hidden sm:inline-block">
                <PaginationLink
                  href={createPageUrl(p)}
                  onClick={(e) => handlePageChange(e, p)}
                  isActive={pageNumber === p}
                >
                  {p}
                </PaginationLink>
              </PaginationItem>
            ))}

            <PaginationItem>
              <PaginationNext
                href={createPageUrl(pageNumber + 1)}
                onClick={(e) => handlePageChange(e, pageNumber + 1)}
                className={
                  pageNumber >= totalPages ? 'pointer-events-none opacity-50' : 'cursor-pointer'
                }
              />
            </PaginationItem>
          </PaginationContent>
        </Pagination>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-5 gap-6 px-10">
        {items.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </div>
  );
}
