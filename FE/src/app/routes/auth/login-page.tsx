import { LoginForm } from "@/features/auth/components/login-form.tsx";
import { useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import { Navigate } from "react-router";

export default function LoginPage() {
  const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);
  return isAuthenticated ? (
    <Navigate to="/" />
  ) : (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        <LoginForm />
      </div>
    </div>
  );
}
