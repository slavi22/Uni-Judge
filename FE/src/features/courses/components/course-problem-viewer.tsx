import { type CourseProblemDto } from "@/features/courses/types/courses-types.ts";
import { Card, CardContent, CardHeader } from "@/components/ui/card.tsx";
import { Link } from "react-router";
import { Button } from "@/components/ui/button.tsx";
import { SquareArrowOutUpRight, SquarePen } from "lucide-react";
import { useAppSelector } from "@/hooks/redux/redux-hooks.ts";

type CourseProblemsViewerProps = {
  data: CourseProblemDto[] | undefined;
  courseId: string | undefined;
};

export default function CourseProblemViewer({
  data,
  courseId,
}: CourseProblemsViewerProps) {
  const { isAuthenticated, roles } = useAppSelector((state) => state.auth);
  return (
    <div className="flex flex-col justify-center items-center gap-5 py-4">
      {data?.map((problem) => (
        <Card
          key={problem.problemId}
          className="w-1/4 transition-all duration-300 hover:scale-105 hover:shadow-lg hover:border-primary/50 cursor-pointer"
        >
          <CardHeader className="font-semibold text-lg">
            <div className="flex gap-3 items-center">
              {problem.name}
              {isAuthenticated && roles.includes("Teacher") && (
                <Link
                  to={`/problems/edit-problem/${courseId}/${problem.problemId}`}
                >
                  <Button variant="outline">
                    <SquarePen />
                  </Button>
                </Link>
              )}
            </div>
          </CardHeader>
          <CardContent>
            <div className="flex flex-col gap-3">
              <Link to={`/courses/${courseId}/${problem.problemId}`}>
                <Button
                  variant="outline"
                  className="flex gap-2 w-full justify-center"
                >
                  <SquareArrowOutUpRight className="h-4 w-4" />
                  <span>View problem</span>
                </Button>
              </Link>
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}
