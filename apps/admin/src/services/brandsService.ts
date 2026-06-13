import axiosInstance from "@/api/axiosInstance";
import { API_ROUTES } from "@/lib/routes";
import { Brand } from "@/types/brand";
import { PaginatedResponse } from "@/types/paginatedResponse";

export interface BrandDTO {
  name: string;
  description: string;
}

interface BrandResponse extends PaginatedResponse<Brand> {
  brands: Brand[];
}

export const brandsService = {
  getAll: (includeDeleted = false, page?: number, pageSize?: number) => {
    return axiosInstance.get<BrandResponse>(API_ROUTES.brands.base, {
      params: {
        includeDeleted,
        page,
        pageSize,
      },
    });
  },
  create: (brand: BrandDTO) => {
    return axiosInstance.post<Brand>(API_ROUTES.brands.base, brand);
  },
  getById: (brandId: string) => {
    return axiosInstance.get<Brand>(API_ROUTES.brands.byId(brandId));
  },
  update: (brandId: string, brand: BrandDTO) => {
    return axiosInstance.put<Brand>(API_ROUTES.brands.byId(brandId), brand);
  },
  softDelete: (brandId: string) => {
    return axiosInstance.delete<Brand>(API_ROUTES.brands.byId(brandId));
  },
  hardDelete: (brandId: string) => {
    return axiosInstance.delete<Brand>(API_ROUTES.brands.hardDelete(brandId));
  },
  restore: (brandId: string) => {
    return axiosInstance.post<Brand>(API_ROUTES.brands.restore(brandId));
  },
};
