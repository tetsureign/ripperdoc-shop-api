import axios from "axios";

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  timeout: 10000,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

import { setupInterceptors } from "./interceptors";
setupInterceptors(axiosInstance);

export default axiosInstance;
