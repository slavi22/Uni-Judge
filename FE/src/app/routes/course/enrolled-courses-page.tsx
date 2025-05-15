import EnrolledCoursesPicker from "@/features/courses/components/enrolled-courses-picker.tsx";
import { useGetEnrolledCoursesQuery } from "@/features/courses/api/course-api.ts";

export default function EnrolledCoursesPage() {
  const { data, isLoading } = useGetEnrolledCoursesQuery(undefined, {
    refetchOnMountOrArgChange: true,
  });
  return <EnrolledCoursesPicker data={data} isLoading={isLoading} />;
}
