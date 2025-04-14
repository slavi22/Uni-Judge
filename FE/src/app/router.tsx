import { createBrowserRouter } from "react-router";
import RootLayout from "@/components/layouts/root-layout.tsx";
import HomePage from "@/app/routes/home-page.tsx";
import LoginPage from "@/app/routes/auth/login-page.tsx";

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
        path: "*",
        element: <LoginPage />,
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
