import { cn } from "@/lib/utils.ts";
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
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";
import InputWithTooltip from "@/components/inputs/input-with-tooltip.tsx";
import { InfoIcon } from "lucide-react";
import { Input } from "@/components/ui/input.tsx";
import { Button } from "@/components/ui/button.tsx";
import { type ComponentProps, useState } from "react";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select.tsx";
import { useGetMyCreatedCoursesQuery } from "@/features/courses/api/course-api.ts";
import ProblemCodeEditorDialog from "@/features/problems/components/problem-code-editor-dialog.tsx";
import { Textarea } from "@/components/ui/textarea.tsx";
import { Slider } from "@/components/ui/slider.tsx";

const formSchema = z.object({
  courseId: z.string().min(1, { message: "Course ID cannot be empty." }),
  problemId: z.string().min(1, { message: "Problem ID cannot be empty." }),
  name: z.string().min(1, { message: "Name cannot be empty." }),
  problemDescription: z
    .string()
    .min(1, { message: "Description cannot be empty." }),
  requiredPercentageToPass: z.number().min(1).max(100),
  mainMethodBodiesList: z.string().min(1, { message: "Code cannot be empty." }),
});

export default function CreateNewProblemForm({
  className,
  ...props
}: ComponentProps<"div">) {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      courseId: "",
      problemId: "",
      name: "",
      problemDescription: "",
      requiredPercentageToPass: 50,
      mainMethodBodiesList: "",
    },
  });

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    console.log(formData);
  }

  const { data } = useGetMyCreatedCoursesQuery();

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">Create Problem</CardTitle>
          <CardDescription>Create a new Problem</CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            {/*{isFetchBaseQueryError(error) && (
              <p className="mb-6 text-destructive">{error.data.detail}</p>
            )}*/}
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
              <FormField
                control={form.control}
                name="courseId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Course ID</FormLabel>
                    <FormControl>
                      <Select
                        onValueChange={field.onChange}
                        value={field.value}
                      >
                        <SelectTrigger className="w-full">
                          <SelectValue placeholder="Select a course" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectGroup>
                            <SelectLabel>
                              Select a course from the ones you've created
                            </SelectLabel>
                            {data &&
                              data.map((item) => (
                                <SelectItem
                                  key={item.courseId}
                                  value={item.courseId}
                                >
                                  {item.name}
                                </SelectItem>
                              ))}
                          </SelectGroup>
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="problemId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Problem ID</FormLabel>
                    <FormControl>
                      <Input placeholder="A unique Problem ID" {...field} />
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
                    <FormLabel>Name</FormLabel>
                    <FormControl>
                      <Input placeholder="Name for the problem" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="problemDescription"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>problemDescription</FormLabel>
                    <FormControl>
                      <Textarea {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="requiredPercentageToPass"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>
                      Required percentage to pass problem - {field.value}%
                    </FormLabel>
                    <FormControl>
                      <Slider
                        min={0}
                        max={100}
                        step={1}
                        defaultValue={[field.value]}
                        onValueChange={(vals) => {
                          field.onChange(vals[0])
                        }}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="mainMethodBodiesList"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>mainMethodBodiesList</FormLabel>
                    <FormControl>
                      <ProblemCodeEditorDialog
                        value={field.value}
                        onChange={field.onChange}
                        inputIsInvalid={
                          !!form.getFieldState("mainMethodBodiesList", form.formState).error
                        }
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              {/*<Button className="w-full" disabled={isLoading}>
                {isLoading ? "Submitting..." : "Create course"}
              </Button>*/}
              <Button className="w-full">Submit</Button>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
