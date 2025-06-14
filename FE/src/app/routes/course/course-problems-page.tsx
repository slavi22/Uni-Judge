import CourseProblemViewer from "@/features/courses/components/course-problem-viewer.tsx";
import { useGetCourseProblemsQuery } from "@/features/courses/api/course-api.ts";
import { useNavigate, useParams } from "react-router";
import { useEffect } from "react";
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";

export default function CourseProblemsPage() {
  const { courseId } = useParams();
  const { data, error } = useGetCourseProblemsQuery(courseId!, {
    refetchOnMountOrArgChange: true,
  });
  const navigate = useNavigate();

  useEffect(() => {
    if (isFetchBaseQueryError(error) && error.status === 404) {
      navigate("/not-found");
    }
    if (isFetchBaseQueryError(error) && error.status === 403) {
      navigate("/forbidden");
    }
  }, [error, navigate]);

  return <CourseProblemViewer data={data} courseId={courseId} />;
}
