import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog.tsx";
import { useSidebar } from "@/components/ui/sidebar.tsx";
import { type AllCoursesDto } from "@/features/courses/types/courses-types.ts";
import { Button } from "@/components/ui/button.tsx";
import {
  BaseQueryFn,
  FetchArgs,
  QueryActionCreatorResult,
  QueryDefinition,
} from "@reduxjs/toolkit/query";
import { FetchBaseQueryError } from "@reduxjs/toolkit/query/react";
import { useState } from "react";
import { useSignUpForCourseMutation } from "@/features/courses/api/course-api.ts";

type CourseSignupPasswordlessDialogProps = {
  course: AllCoursesDto;
  refetchPageFn: () => QueryActionCreatorResult<
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

export default function CourseSignupPasswordlessDialog({
  course,
  refetchPageFn
}: CourseSignupPasswordlessDialogProps) {
  const { open } = useSidebar();
  const [dialogIsOpen, setDialogIsOpen] = useState(false);
  const [signUpForCourse] = useSignUpForCourseMutation();

  async function handleSignUp() {
    const result = await signUpForCourse({
      courseId: course.courseId,
      password: undefined,
    });
    if (!result.error) {
      refetchPageFn();
    }
  }

  return (
    <Dialog open={dialogIsOpen} onOpenChange={setDialogIsOpen}>
      <DialogTrigger asChild>
        <p className="flex-1">
          Sign up for the{" "}
          <span className="font-medium underline underline-offset-2">
            {course.name}
          </span>{" "}
          course
        </p>
      </DialogTrigger>
      <DialogContent className={open ? "md:ml-[125px]" : undefined}>
        <DialogHeader>
          <DialogTitle>
            Sign up for the{" "}
            <span className="font-medium underline underline-offset-2">
              {course.name}
            </span>{" "}
            course
          </DialogTitle>
          <DialogDescription>
            You must sign up for the course to access its content.
          </DialogDescription>
        </DialogHeader>
        <div>
          <p>This course doesn't require a password to sign up.</p>
          <p>If you wish to sign up for it click the button below.</p>
          <Button className="w-full mt-3" onClick={handleSignUp}>
            Sign up
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
}
