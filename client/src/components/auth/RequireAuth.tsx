import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "./hooks";

export const RequireAuth = () => {
  const auth = useAuth();
  const location = useLocation();

  if (auth.loading) return <></>;

  if (!auth.user) {
    return <Navigate to="/sign-up" state={{ from: location }} replace />;
  }

  return <Outlet />;
}
