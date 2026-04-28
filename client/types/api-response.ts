export interface ApiResponse<T> {
  data: T | null;
  error: ApiError | null;
  statusCode: number;
}

export interface ApiError {
  message: string;
  statusCode: number;
  details: string | null;
}
