import { baseApi } from "@/stores/base-api.ts";
import {
  AllCoursesDto,
  CourseProblemDto,
  EnrolledCoursesDto,
  type NewCourseDto,
  SignUpForCourseDto,
  TeacherCoursesDto,
} from "@/features/courses/types/courses-types.ts";
import { toast } from "sonner";

const courseApi = baseApi.injectEndpoints({
  endpoints: (build) => ({
    createNewCourse: build.mutation<void, NewCourseDto>({
      query: (newCourseData) => ({
        url: "courses/create-new-course",
        method: "POST",
        body: newCourseData,
      }),
      async onQueryStarted(_args, { queryFulfilled }) {
        try {
          await queryFulfilled;
          toast.success("Course created successfully.");
        } catch {
          toast.error("Error creating course.");
        }
      },
    }),
    getMyCreatedCourses: build.query<TeacherCoursesDto[], void>({
      query: () => "courses/get-my-created-courses",
    }),
    getAllCourses: build.query<AllCoursesDto[], void>({
      query: () => "courses/get-all-courses",
    }),
    signUpForCourse: build.mutation<void, SignUpForCourseDto>({
      query: (signUpData) => ({
        url: "courses/signup-for-course",
        method: "POST",
        body: signUpData,
      }),
      async onQueryStarted(_args, { queryFulfilled }) {
        try {
          await queryFulfilled;
          toast.success("Successfully signed up for the course.");
        } catch {
          toast.error("Error signing up for the course.");
        }
      },
    }),
    getEnrolledCourses: build.query<EnrolledCoursesDto[], void>({
      query: () => "courses/get-enrolled-courses",
    }),
    getCourseProblems: build.query<CourseProblemDto[], string>({
      query: (courseId) => `courses/course/${courseId}`,
    }),
  }),
});

export const {
  useCreateNewCourseMutation,
  useGetMyCreatedCoursesQuery,
  useGetAllCoursesQuery,
  useSignUpForCourseMutation,
  useGetEnrolledCoursesQuery,
  useGetCourseProblemsQuery,
} = courseApi;
