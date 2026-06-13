import { AxiosInstance } from "axios";
import { APP_ROUTES, APP_ROUTE_PREFIX, API_ROUTES } from "@/lib/routes";

export const setupInterceptors = (axiosInstance: AxiosInstance) => {
  axiosInstance.interceptors.response.use(
    (response) => response,
    (error) => {
      const originalRequest = error.config;

      const isLoginAttempt = originalRequest?.url?.includes(
        API_ROUTES.auth.login
      );
      const status = error?.response?.status;

      if (status === 401 && !isLoginAttempt) {
        const loginUrl = `${APP_ROUTE_PREFIX}${APP_ROUTES.login.url}`;
        console.warn(`[api] 401 Unauthorized, redirecting to ${loginUrl}...`);
        window.location.href = loginUrl;
      } else if (status === 401 && isLoginAttempt)
        console.error("Invalid creds, choom.");
      else console.error("[api] Error: ", error.response);

      return Promise.reject(error);
    }
  );
};
