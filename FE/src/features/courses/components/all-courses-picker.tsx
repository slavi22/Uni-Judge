import { Card, CardContent, CardHeader } from "@/components/ui/card.tsx";
import { Lock, LockOpen, SquareArrowOutUpRight } from "lucide-react";
import InfoTooltip from "@/components/tooltips/info-tooltip.tsx";
import { Button } from "@/components/ui/button.tsx";
import { cn } from "@/lib/utils.ts";
import { Link } from "react-router";
import CourseSignupPasswordDialog from "@/features/courses/components/course-signup-password-dialog.tsx";
import CourseSignupPasswordlessDialog from "@/features/courses/components/course-signup-passwordless-dialog.tsx";
import { type AllCoursesDto } from "../types/courses-types";
import {
  BaseQueryFn,
  FetchArgs,
  QueryActionCreatorResult,
  QueryDefinition,
} from "@reduxjs/toolkit/query";
import { FetchBaseQueryError } from "@reduxjs/toolkit/query/react";

type ALlCoursesPickerProps = {
  data: AllCoursesDto[] | undefined;
  refetch: () => QueryActionCreatorResult<
    QueryDefinition<
      void,
      BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError>,
      never,
      AllCoursesDto[],
      "baseApi",
      unknown
    >
  >;
};

export default function AllCoursesPicker({
  data,
  refetch,
}: ALlCoursesPickerProps) {
  return (
    <div className="flex flex-col justify-center items-center gap-5 py-4">
      {data?.map((course) => (
        <Card
          key={course.courseId}
          className="w-1/3 transition-all duration-300 hover:scale-105 hover:shadow-lg hover:border-primary/50 cursor-pointer"
        >
          <CardHeader className="font-semibold text-lg">
            {course.name}
          </CardHeader>
          <CardContent>
            <div className="flex flex-col gap-3">
              {course.userIsEnrolled ? (
                <Link to={`/courses/${course.courseId}`}>
                  <Button
                    variant="outline"
                    className="flex gap-2 w-full justify-center"
                  >
                    <SquareArrowOutUpRight className="h-4 w-4" />
                    <span>Go to course</span>
                  </Button>
                </Link>
              ) : (
                <div
                  className={cn(
                    "p-3 rounded-md transition-all duration-300",
                    "border border-transparent hover:border-primary/40 hover:bg-primary/5",
                  )}
                >
                  {course.isPasswordProtected ? (
                    <div className="flex items-center gap-2">
                      <CourseSignupPasswordDialog
                        course={course}
                        refetchPageFn={refetch}
                      />
                      <InfoTooltip
                        icon={Lock}
                        tooltipContent="This course is password protected"
                      />
                    </div>
                  ) : (
                    <div className="flex items-center gap-2">
                      <CourseSignupPasswordlessDialog
                        course={course}
                        refetchPageFn={refetch}
                      />
                      <InfoTooltip
                        icon={LockOpen}
                        tooltipContent="This course is open"
                      />
                    </div>
                  )}
                </div>
              )}
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}
