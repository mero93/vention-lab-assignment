const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export const API_ENDPOINTS = {
  PRODUCTS: `${API_URL}/api/products`,
  UPLOAD: `${API_URL}/api/upload`,
};

export const STORAGE_PATHS = {
  UPLOADS: `${API_URL}/uploads`,
};
