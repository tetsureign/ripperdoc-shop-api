import { useNavigate } from "react-router";
import { APP_ROUTES } from "@/lib/routes";
import { authService } from "@/services/authService";

export const useLogout = () => {
  const navigate = useNavigate();

  const logout = async () => {
    try {
      const res = await authService.logout();
      console.log("Jack out successful", res.data);
    } catch (err) {
      console.error("Jack out failed", err);
    } finally {
      // navigate(APP_ROUTES.login.url);
      navigate(APP_ROUTES.login.url, { replace: true });
    }
  };

  return logout;
};
