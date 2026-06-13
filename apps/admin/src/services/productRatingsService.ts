import axiosInstance from "@/api/axiosInstance";
import { API_ROUTES } from "@/lib/routes";

export const productRatingsService = {
  getByProductId: (productId: string) => {
    return axiosInstance.get(API_ROUTES.productRatings.byProductId(productId));
  },
  getByUserId: (userId: string) => {
    return axiosInstance.get(API_ROUTES.productRatings.byUserId(userId));
  },
  getById: (ratingId: string) => {
    return axiosInstance.get(API_ROUTES.productRatings.byId(ratingId));
  },
  delete: (ratingId: string) => {
    return axiosInstance.delete(API_ROUTES.productRatings.byId(ratingId));
  },
  restore: (ratingId: string) => {
    return axiosInstance.post(API_ROUTES.productRatings.restore(ratingId));
  },
};
