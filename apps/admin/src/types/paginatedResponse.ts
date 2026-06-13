export interface PaginatedResponse<T> {
  result: T[];
  totalCount: number;
  totalPages: number;
}
