import { createBrowserRouter } from "react-router";
import RootLayout from "@/components/layouts/root-layout.tsx";
import HomePage from "@/app/routes/home-page.tsx";
import LoginPage from "@/app/routes/auth/login-page.tsx";
import ProtectedRoute from "@/components/protected/protected-route.tsx";
import RegisterPage from "@/app/routes/auth/register-page.tsx";
import RegisterTeacherPage from "@/app/routes/auth/register-teacher-page.tsx";
import TeacherProtectedRoute from "@/components/protected/teacher-protected-route.tsx";
import CreateNewCoursePage from "@/app/routes/course/create-new-course-page.tsx";
import CreateNewProblemPage from "@/app/routes/problems/create-new-problem-page.tsx";

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
            path: "me",
            element: <div>My profile</div>,
          },
          {
            path: "courses",
            children: [
              {
                path: "all-courses",
                element: <div>All Courses</div>,
              },
              {
                element: <TeacherProtectedRoute />,
                children: [
                  {
                    path: "create-new-course",
                    element: <CreateNewCoursePage />,
                  },
                ],
              },
            ],
          },
          {
            path: "problems",
            children: [
              {
                element: <TeacherProtectedRoute />,
                children: [
                  {
                    path: "create-new-problem",
                    element: <CreateNewProblemPage />,
                  },
                ],
              },
            ],
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
