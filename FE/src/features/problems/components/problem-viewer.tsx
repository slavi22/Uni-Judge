import CodeEditorProblem from "@/features/code-editor/components/code-editor-problem.tsx";
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
  return <CodeEditorProblem data={data}/>;
}
