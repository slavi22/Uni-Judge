import { baseApi } from "@/stores/base-api.ts";
import { type NewCourseDto, TeacherCoursesDto } from "@/features/courses/types/courses-types.ts";
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
    })
  }),
});

export const { useCreateNewCourseMutation, useGetMyCreatedCoursesQuery } = courseApi;
