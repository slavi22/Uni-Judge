import LastUserSubmissionsTable from "@/features/submissions/components/last-user-submissions-table.tsx";
import { useNavigate, useParams, useSearchParams } from "react-router";
import { useGetLastUserSubmissionsForProblemQuery } from "@/features/submissions/api/submissions-api.ts";
import { useEffect, useState } from "react";
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";
import LoadingSpinner from "@/components/spinners/loading-spinner.tsx";

export default function LastUserSubmissionsPage() {
  const { courseId, problemId } = useParams();
  const [queryParam, setQueryParam] = useSearchParams();
  const [selectValue, setSelectValue] = useState<string>(
    queryParam.get("numOfSubmissions") || "",
  );
  const { data, error, isLoading } = useGetLastUserSubmissionsForProblemQuery({
    courseId: courseId!,
    problemId: problemId!,
    numOfSubmissions: Number(selectValue),
  });
  const navigate = useNavigate();
  useEffect(() => {
    if (isFetchBaseQueryError(error) && error.status === 404) {
      navigate("/not-found");
    }
  }, [error, navigate]);
  return isLoading ? (
    <LoadingSpinner text="Loading data..." />
  ) : (
    <LastUserSubmissionsTable
      data={data}
      selectValue={selectValue}
      setSelectValue={(value: string) => setSelectValue(value)}
      setQueryParamValue={(numOfSubmissions: string) =>
        setQueryParam(searchParams => {
          searchParams.set("numOfSubmissions", numOfSubmissions)
          return searchParams;
        })
      }
    />
  );
}
