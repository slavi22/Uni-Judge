import ProblemViewer from "@/features/problems/components/problem-viewer.tsx";
import { useGetProblemInfoQuery } from "@/features/problems/api/problems-api.ts";
import { useNavigate, useParams } from "react-router";
import { useEffect } from "react";
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";

export default function ProblemPage() {
  const { courseId, problemId } = useParams();
  const { data, error, isLoading } = useGetProblemInfoQuery({
    courseId: courseId!,
    problemId: problemId!,
  });

  const navigate = useNavigate();
  useEffect(() => {
    if (isFetchBaseQueryError(error) && error.status === 404) {
      navigate("/not-found");
    }
  }, [error, navigate]);

  //TODO: make the loading state more elegant
  return isLoading ? <p>Loading</p> : <ProblemViewer data={data} courseId={courseId} problemId={problemId} />
}
