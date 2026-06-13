import axiosInstance from "@/api/axiosInstance";
import { API_ROUTES } from "@/lib/routes";
import { Category } from "@/types/category";
import { PaginatedResponse } from "@/types/paginatedResponse";

export interface CategoryDTO {
  name: string;
  description: string;
}

interface CategoryResponse extends PaginatedResponse<Category> {
  categories: Category[];
}

export const categoriesService = {
  getAll: (includeDeleted = false, page?: number, pageSize?: number) => {
    return axiosInstance.get<CategoryResponse>(API_ROUTES.categories.base, {
      params: {
        includeDeleted,
        page,
        pageSize,
      },
    });
  },
  create: (category: CategoryDTO) => {
    return axiosInstance.post<Category>(API_ROUTES.categories.base, category);
  },
  getById: (categoryId: string) => {
    return axiosInstance.get<Category>(API_ROUTES.categories.byId(categoryId));
  },
  update: (categoryId: string, category: CategoryDTO) => {
    return axiosInstance.put<Category>(
      API_ROUTES.categories.byId(categoryId),
      category
    );
  },
  softDelete: (categoryId: string) => {
    return axiosInstance.delete<Category>(
      API_ROUTES.categories.byId(categoryId)
    );
  },
  hardDelete: (categoryId: string) => {
    return axiosInstance.delete<Category>(
      API_ROUTES.categories.hardDelete(categoryId)
    );
  },
  restore: (categoryId: string) => {
    return axiosInstance.post<Category>(
      API_ROUTES.categories.restore(categoryId)
    );
  },
};
