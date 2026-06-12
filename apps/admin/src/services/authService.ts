import axiosInstance from "@/api/axiosInstance";
import { API_ROUTES } from "@/lib/routes";

export interface User {
  username: string;
  roles: string[];
}

export const authService = {
  login: (credentials: { email: string; password: string }) => {
    return axiosInstance.post<User>(API_ROUTES.auth.login, credentials);
  },
  logout: () => {
    return axiosInstance.post(API_ROUTES.auth.logout);
  },
  whoami: () => {
    return axiosInstance.get<User>(API_ROUTES.auth.whoami);
  },
};
