import { useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import { Navigate, Outlet } from "react-router";

export default function AdminProtectedRoute() {
  const { isAuthenticated, roles } = useAppSelector((state) => state.auth);
  return isAuthenticated && roles.includes("Admin") ? (
    <Outlet />
  ) : (
    <Navigate to="/login" replace />
  );
}
