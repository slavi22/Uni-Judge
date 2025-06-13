import { Card, CardContent, CardHeader } from "@/components/ui/card.tsx";
import { Link } from "react-router";
import { Button } from "@/components/ui/button.tsx";
import { SquareArrowOutUpRight } from "lucide-react";
import { type EnrolledCoursesDto } from "@/features/courses/types/courses-types.ts";
import LoadingSpinner from "@/components/spinners/loading-spinner.tsx";

type EnrolledCoursesPickerProps = {
  data: EnrolledCoursesDto[] | undefined;
  isLoading: boolean;
};

export default function EnrolledCoursesPicker({
  data,
  isLoading,
}: EnrolledCoursesPickerProps) {
  return (
    <div className="flex flex-col justify-center items-center gap-5 py-4">
      {isLoading ? (
        <LoadingSpinner text="Loading courses..." />
      ) : data?.length ? (
        data?.map((course) => (
          <Card
            key={course.courseId}
            className="w-1/3 transition-all duration-300 hover:scale-105 hover:shadow-lg hover:border-primary/50 cursor-pointer"
          >
            <CardHeader className="font-semibold text-lg">
              {course.name}
            </CardHeader>
            <CardContent>
              <div className="flex flex-col gap-3">
                <Link to={`/courses/${course.courseId}`}>
                  <Button
                    variant="outline"
                    className="flex gap-2 w-full justify-center"
                  >
                    <SquareArrowOutUpRight className="h-4 w-4" />
                    <span>Go to course</span>
                  </Button>
                </Link>
              </div>
            </CardContent>
          </Card>
        ))
      ) : (
        <>
          <p>You have not signed up for any courses yet.</p>
          <span>
            {" "}
            You can do so{" "}
            <Link
              to="/courses"
              className="underline underline-offset-2"
            >
              Here
            </Link>
          </span>
        </>
      )}
    </div>
  );
}
