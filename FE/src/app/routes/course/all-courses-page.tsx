import AllCoursesPicker from "@/features/courses/components/all-courses-picker.tsx";
import { useGetAllCoursesQuery } from "@/features/courses/api/course-api.ts";

export default function AllCoursesPage() {
  const { data, refetch } = useGetAllCoursesQuery(undefined, {
    refetchOnMountOrArgChange: true,
  });
  return <AllCoursesPicker data={data} refetch={refetch}/>;
}
