import { ApiResponse } from '@/types/api-response';
import { PaginatedResult } from '@/types/paginated-result';
import { Product } from '@/types/product';
import { API_ENDPOINTS } from './constants';

export async function getProducts(
  pageNumber: number,
  title: string,
): Promise<ApiResponse<PaginatedResult<Product>>> {
  try {
    const res = await fetch(
      `${API_ENDPOINTS.PRODUCTS}?pageNumber=${pageNumber}&pageSize=10&title=${title}`,
      {
        cache: 'no-store',
      },
    );

    const result: ApiResponse<PaginatedResult<Product>> = await res.json();

    return result;
  } catch {
    return {
      data: null,
      error: { message: 'Failed to fetch products', statusCode: 500, details: null },
      statusCode: 500,
    };
  }
}

export async function uploadImage(file: File): Promise<ApiResponse<{ fileName: string }>> {
  try {
    const formData = new FormData();
    formData.append('file', file);

    const res = await fetch(API_ENDPOINTS.UPLOAD, {
      method: 'POST',
      body: formData,
    });

    const result = await res.json();
    return result;
  } catch {
    return {
      data: null,
      error: { message: 'Image upload failed', statusCode: 500, details: null },
      statusCode: 500,
    };
  }
}

export async function createProduct(
  productData: Omit<Product, 'id'>,
): Promise<ApiResponse<Product>> {
  try {
    const res = await fetch(API_ENDPOINTS.PRODUCTS, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(productData),
    });

    return await res.json();
  } catch {
    return {
      data: null,
      error: { message: 'Failed to create a product', statusCode: 500, details: null },
      statusCode: 500,
    };
  }
}
