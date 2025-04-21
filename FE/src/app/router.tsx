import { createBrowserRouter } from "react-router";
import RootLayout from "@/components/layouts/root-layout.tsx";
import HomePage from "@/app/routes/home-page.tsx";
import LoginPage from "@/app/routes/auth/login-page.tsx";
import ProtectedRoute from "@/components/protected/protected-route.tsx";
import RegisterPage from "@/app/routes/auth/register-page.tsx";
import RegisterTeacherPage from "@/app/routes/auth/register-teacher-page.tsx";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <RootLayout />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: "/login",
        element: <LoginPage />,
      },
      {
        path: "/register",
        element: <RegisterPage />,
      },
      {
        path: "/register-teacher",
        element: <RegisterTeacherPage />,
      },
      {
        element: <ProtectedRoute />,
        children: [
          {
            path: "/me",
            element: <div>My profile</div>,
          },
        ],
      },
      {
        path: "test-breadcrumb",
        element: <div>BreadCrumb Test Home</div>,
        children: [
          {
            path: "first-level",
            element: <div>BreadCrumb Test First level</div>,
          },
        ],
      },
    ],
  },
]);
