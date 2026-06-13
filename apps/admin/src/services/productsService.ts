import axiosInstance from "@/api/axiosInstance";
import { API_ROUTES } from "@/lib/routes";
import { PaginatedResponse } from "@/types/paginatedResponse";
import { Product } from "@/types/product";

export interface ProductDTO {
  id?: string;
  name: string;
  description: string;
  imageUrl: string;
  price: number;
  isFeatured: boolean;
  categoryId: string;
  brandId: string | null;
}

interface ProductResponse extends PaginatedResponse<Product> {
  products: Product[];
}

export const productsService = {
  getAll: (includeDeleted = false, page?: number, pageSize?: number) => {
    return axiosInstance.get<ProductResponse>(API_ROUTES.products.base, {
      params: {
        includeDeleted,
        page,
        pageSize,
      },
    });
  },
  create: (product: ProductDTO) => {
    return axiosInstance.post<ProductDTO>(API_ROUTES.products.base, product);
  },
  getById: (productId: string) => {
    return axiosInstance.get<Product>(API_ROUTES.products.byId(productId));
  },
  update: (productId: string, product: ProductDTO) => {
    return axiosInstance.put<ProductDTO>(
      API_ROUTES.products.byId(productId),
      product
    );
  },
  feature: (productId: string) => {
    return axiosInstance.post<Product>(API_ROUTES.products.feature(productId));
  },
  unfeature: (productId: string) => {
    return axiosInstance.post<Product>(
      API_ROUTES.products.unfeature(productId)
    );
  },
  softDelete: (productId: string) => {
    return axiosInstance.delete<Product>(API_ROUTES.products.byId(productId));
  },
  hardDelete: (productId: string) => {
    return axiosInstance.delete<Product>(
      API_ROUTES.products.hardDelete(productId)
    );
  },
  restore: (productId: string) => {
    return axiosInstance.post<Product>(API_ROUTES.products.restore(productId));
  },
};
