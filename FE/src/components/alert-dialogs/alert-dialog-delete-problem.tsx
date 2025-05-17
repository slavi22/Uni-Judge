import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { Button, buttonVariants } from "@/components/ui/button.tsx";
import { Trash2 } from "lucide-react";
import { useSidebar } from "@/components/ui/sidebar.tsx";
import { useDeleteCourseMutation } from "@/features/courses/api/course-api.ts";
import type { FetchBaseQueryError, QueryActionCreatorResult } from "@reduxjs/toolkit/query/react";
import type { BaseQueryFn, FetchArgs, QueryDefinition } from "@reduxjs/toolkit/query";
import type { AllCoursesDto } from "@/features/courses/types/courses-types.ts";

type AlertDialogDeleteProblemProps = {
  courseId: string;
  refetchParent: () => QueryActionCreatorResult<
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

export default function AlertDialogDeleteProblem({
  courseId,
  refetchParent,
}: AlertDialogDeleteProblemProps) {
  const { open } = useSidebar();
  const [deleteProblem] = useDeleteCourseMutation();

  function handleDelete() {
    deleteProblem(courseId);
    refetchParent();
  }

  return (
    <AlertDialog>
      <AlertDialogTrigger asChild>
        <Button variant="destructive">
          <Trash2 />
        </Button>
      </AlertDialogTrigger>
      <AlertDialogContent className={open ? "md:ml-[125px]" : undefined}>
        <AlertDialogHeader>
          <AlertDialogTitle>
            Are you sure you want to delete this course?
          </AlertDialogTitle>
          <AlertDialogDescription>
            This action cannot be undone.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Cancel</AlertDialogCancel>
          <AlertDialogAction
            className={buttonVariants({ variant: "destructive" })}
            onClick={handleDelete}
          >
            Delete
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
