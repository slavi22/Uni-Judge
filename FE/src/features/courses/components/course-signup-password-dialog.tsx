import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog.tsx";
import { type AllCoursesDto } from "@/features/courses/types/courses-types.ts";
import { useSidebar } from "@/components/ui/sidebar.tsx";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form.tsx";
import PasswordInput from "@/components/inputs/password-input.tsx";
import { Button } from "@/components/ui/button.tsx";
import { useSignUpForCourseMutation } from "@/features/courses/api/course-api.ts";
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";
import { useState } from "react";
import {
  BaseQueryFn,
  FetchArgs,
  QueryActionCreatorResult,
  QueryDefinition,
} from "@reduxjs/toolkit/query";
import { FetchBaseQueryError } from "@reduxjs/toolkit/query/react";

type CourseSignupDialogProps = {
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

const formSchema = z.object({
  password: z.string().min(1, { message: "Password is required." }),
});

export default function CourseSignupPasswordDialog({
  course,
  refetchPageFn
}: CourseSignupDialogProps) {
  const { open } = useSidebar();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      password: "",
    },
  });

  const [dialogIsOpen, setDialogIsOpen] = useState(false);
  const [signUpForCourse, { error }] = useSignUpForCourseMutation();

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    const result = await signUpForCourse({
      courseId: course.courseId,
      password: formData.password,
    });

    if (!result.error) {
      setDialogIsOpen(false);
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
          {error && (
            <p className="text-destructive mb-3">
              {isFetchBaseQueryError(error) && error.data.detail}
            </p>
          )}
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
              <div className="flex items-center gap-2">
                <FormField
                  control={form.control}
                  name="password"
                  render={({ field }) => (
                    <FormItem className="w-full">
                      <FormLabel>Course Password</FormLabel>
                      <div className="flex w-full gap-2">
                        <div className="w-full">
                          <FormControl>
                            <PasswordInput
                              placeholder="Course password"
                              {...field}
                            />
                          </FormControl>
                        </div>
                        <Button>Sign up</Button>
                      </div>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
            </form>
          </Form>
        </div>
      </DialogContent>
    </Dialog>
  );
}
