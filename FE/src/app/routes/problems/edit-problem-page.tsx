import EditProblemForm from "@/features/problems/components/edit-problem-form.tsx";
import { useParams } from "react-router";
import { useGetEditProblemInfoQuery } from "@/features/problems/api/problems-api.ts";
import LoadingSpinner from "@/components/spinners/loading-spinner.tsx";

export default function EditProblemPage() {
  const { courseId, problemId } = useParams();
  const { data, isLoading } = useGetEditProblemInfoQuery({
    courseId: courseId!,
    problemId: problemId!,
  });
  return (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        {isLoading ? (
          <LoadingSpinner text="Loading..." />
        ) : (
          <EditProblemForm
            data={data}
            courseId={courseId}
            problemId={problemId}
          />
        )}
      </div>
    </div>
  );
}
