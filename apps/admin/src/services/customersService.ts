import axiosInstance from "@/api/axiosInstance";
import { API_ROUTES } from "@/lib/routes";
import { PaginatedResponse } from "@/types/paginatedResponse";
import { Customer } from "@/types/customer";

interface CustomerResponse extends PaginatedResponse<Customer> {
  customers: Customer[];
}

export const customersService = {
  getAll: (includeDeleted = false, page?: number, pageSize?: number) => {
    return axiosInstance.get<CustomerResponse>(API_ROUTES.customers.base, {
      params: {
        includeDeleted,
        page,
        pageSize,
      },
    });
  },
};
