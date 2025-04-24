import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card.tsx";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form.tsx";
import { cn } from "@/lib/utils.ts";
import { type ComponentProps } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";
import { Input } from "@/components/ui/input.tsx";
import { InfoIcon } from "lucide-react";
import InputWithTooltip from "@/features/courses/components/input-with-tooltip.tsx";
import { Button } from "@/components/ui/button.tsx";
import { useCreateNewCourseMutation } from "@/features/courses/api/course-api.ts";
import { useNavigate } from "react-router";

const formSchema = z.object({
  courseId: z.string().min(1, { message: "Course ID cannot be empty." }),
  name: z.string().min(1, { message: "Course name cannot be empty." }),
  description: z
    .string()
    .min(1, { message: "Course description cannot be empty." }),
  password: z
    .string()
    .min(3, { message: "Password must be at least 3 characters long." })
    // zod optional field => https://stackoverflow.com/a/74546728
    // zod optional vs nullable vs nullish => https://gist.github.com/ciiqr/ee19e9ff3bb603f8c42b00f5ad8c551e
    .optional()
    .or(z.literal("")),
});

export default function CreateNewCourseForm({
  className,
  ...props
}: ComponentProps<"div">) {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      courseId: "",
      name: "",
      description: "",
      password: "",
    },
  });
  const navigate = useNavigate();

  const [createCourse, { error, isLoading }] = useCreateNewCourseMutation();

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    //console.log(formData);
    const result = await createCourse({
      ...formData,
      password: formData.password || null,
    });
    if (!result.error) {
      navigate("/");
    }
  }

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">Create Course</CardTitle>
          <CardDescription>Create a new Course</CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            {isFetchBaseQueryError(error) && (
              <p className="mb-6 text-destructive">{error.data.detail}</p>
            )}
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
              <FormField
                control={form.control}
                name="courseId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Course ID</FormLabel>
                    <FormControl>
                      <InputWithTooltip
                        placeholder="Course Idetifier"
                        tooltipContent="Create a unique, descriptive course ID"
                        icon={InfoIcon}
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Course Name</FormLabel>
                    <FormControl>
                      <Input placeholder="Course Name" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Course Description</FormLabel>
                    <FormControl>
                      <Input placeholder="Course Description" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Course Password</FormLabel>
                    <FormControl>
                      <InputWithTooltip
                        placeholder="Course Password"
                        tooltipContent="Password is optional. Leave empty if you want to make the course public."
                        icon={InfoIcon}
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <Button className="w-full" disabled={isLoading}>
                {isLoading ? "Submitting..." : "Create course"}
              </Button>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
