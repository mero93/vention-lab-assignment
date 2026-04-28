import ProductLibrary from '@/components/ProductLibrary';
import { StatusDisplay } from '@/components/StatusDisplay';
import { API_ENDPOINTS } from '@/lib/constants';
import { ApiResponse } from '@/types/api-response';
import { PaginatedResult } from '@/types/paginated-result';
import { Product } from '@/types/product';
import { AlertTriangle } from 'lucide-react';

export default async function Home({
  searchParams,
}: Readonly<{
  searchParams: Promise<{ pageNumber?: string; title?: string }>;
}>) {
  const params = await searchParams;

  const pageNumber = Math.max(Number(params.pageNumber) || 1, 1);
  const title = params.title || '';

  const url = new URL(API_ENDPOINTS.PRODUCTS);
  url.searchParams.set('pageNumber', pageNumber.toString());
  url.searchParams.set('pageSize', '10');
  if (title) url.searchParams.set('title', title);

  let data: PaginatedResult<Product> | null = null;
  let errorMessage: string | undefined;

  try {
    const res = await fetch(url.toString(), { cache: 'no-store' });

    const result: ApiResponse<PaginatedResult<Product>> = await res.json();

    const { data: receivedData, error } = result;
    data = receivedData;

    if (data?.items.length === 0) {
      errorMessage = 'No items to display';
    }

    errorMessage = error?.message;
  } catch {
    errorMessage = 'Connection to API failed';
  }

  if (errorMessage)
    return (
      <div className="container mx-auto p-6">
        <StatusDisplay
          icon={AlertTriangle}
          title="Something went wrong"
          message={errorMessage}
          variant="error"
        />
      </div>
    );

  return data && <ProductLibrary paginatedResult={data}></ProductLibrary>;
}
