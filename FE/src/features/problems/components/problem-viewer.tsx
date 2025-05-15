import CodeEditorProblem from "@/features/submissions/components/code-editor-problem.tsx";
import { ProblemInfoDto } from "@/features/problems/types/problems-types.ts";

type ProblemViewerProps = {
  data: ProblemInfoDto | undefined;
  courseId: string | undefined;
  problemId: string | undefined;
};

export default function ProblemViewer({
  data,
  courseId,
  problemId,
}: ProblemViewerProps) {
  return (
    <CodeEditorProblem data={data} courseId={courseId} problemId={problemId} />
  );
}
