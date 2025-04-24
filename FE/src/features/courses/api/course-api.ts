import { baseApi } from "@/stores/base-api.ts";
import { NewCourse } from "@/features/courses/types/courses-types.ts";
import { toast } from "sonner";

const courseApi = baseApi.injectEndpoints({
  endpoints: (build) => ({
    createNewCourse: build.mutation<void, NewCourse>({
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
  }),
});

export const { useCreateNewCourseMutation } = courseApi;
