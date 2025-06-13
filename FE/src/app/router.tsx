import { createBrowserRouter, Navigate } from "react-router";
import RootLayout from "@/components/layouts/root-layout.tsx";
import HomePage from "@/app/routes/home-page.tsx";
import LoginPage from "@/app/routes/auth/login-page.tsx";
import ProtectedRoute from "@/components/protected/protected-route.tsx";
import RegisterPage from "@/app/routes/auth/register-page.tsx";
import RegisterTeacherPage from "@/app/routes/auth/register-teacher-page.tsx";
import TeacherProtectedRoute from "@/components/protected/teacher-protected-route.tsx";
import CreateNewCoursePage from "@/app/routes/course/create-new-course-page.tsx";
import CreateNewProblemPage from "@/app/routes/problems/create-new-problem-page.tsx";
import EnrolledCoursesPage from "@/app/routes/course/enrolled-courses-page.tsx";
import AllCoursesPage from "@/app/routes/course/all-courses-page.tsx";
import CourseProblemsPage from "@/app/routes/course/course-problems-page.tsx";
import ProblemPage from "@/app/routes/problems/problem-page.tsx";
import EditProblemPage from "@/app/routes/problems/edit-problem-page.tsx";
import AdminProtectedRoute from "@/components/protected/admin-protected-route.tsx";
import DeleteCoursePage from "@/app/routes/course/delete-course-page.tsx";
import LastUserSubmissionsPage from "@/app/routes/course/last-user-submissions-page.tsx";

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
                index: true,
                element: <AllCoursesPage />,
              },
              {
                path: "enrolled-courses",
                element: <EnrolledCoursesPage />,
              },
              {
                path: ":courseId",
                element: <CourseProblemsPage />,
              },
              {
                path: ":courseId/:problemId",
                element: <ProblemPage />,
              },
              {
                element: <TeacherProtectedRoute />,
                children: [
                  {
                    path: "create-new-course",
                    element: <CreateNewCoursePage />,
                  },
                  {
                    path: ":courseId/:problemId/last-submissions",
                    element: <LastUserSubmissionsPage />,
                  },
                ],
              },
            ],
          },
          {
            path: "problems",
            children: [
              {
                index: true,
                element: <Navigate to="/courses"/>,
              },
              {
                element: <TeacherProtectedRoute />,
                children: [
                  {
                    path: "create-new-problem",
                    element: <CreateNewProblemPage />,
                  },
                  {
                    path: "edit-problem/:courseId/:problemId",
                    element: <EditProblemPage />,
                  },
                ],
              },
              {
                element: <AdminProtectedRoute />,
                children: [
                  {
                    path: "delete-problems",
                    element: <DeleteCoursePage />,
                  },
                ],
              },
            ],
          },
        ],
      },
      {
        path: "/not-found",
        element: <div>Not found</div>, //TODO: finish
      },
      {
        path: "/forbidden",
        element: <div>Forbidden</div>, //TODO: finish
      },
    ],
  },
]);
