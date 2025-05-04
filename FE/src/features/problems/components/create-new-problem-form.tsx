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
import { Input } from "@/components/ui/input.tsx";
import { Button } from "@/components/ui/button.tsx";
import { type ComponentProps, useEffect } from "react";
import { z } from "zod";
import { useFieldArray, useForm } from "react-hook-form";
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
import ProblemInfoDialog from "@/features/problems/components/problem-info-dialog.tsx";
import { Textarea } from "@/components/ui/textarea.tsx";
import { Slider } from "@/components/ui/slider.tsx";
import { useAppDispatch } from "@/hooks/redux/redux-hooks.ts";
import { clearSolutionDialogData } from "@/features/problems/stores/problem-solutions-slice.ts";
import { type ProblemSolutionsRHFFieldErrors } from "@/features/problems/types/problems-types.ts";
import ExpectedOutputsStdinsDialog from "@/features/problems/components/expected-outputs-stdins-dialog.tsx";

const problemSolutionsSchema = z.object({
  languageId: z.number().min(1, { message: "Language ID cannot be empty." }),
  solutionTemplate: z.string().min(1, {
    message: "Solution template cannot be empty.",
  }),
  mainMethodBodyContent: z.string().min(1, {
    message: "Main method body content cannot be empty.",
  }),
});

const expectedOutputAndStdInSchema = z.object({
  stdInParam: z.string().min(1, { message: "StdIn cannot be empty." }),
  expectedOutput: z
    .string()
    .min(1, { message: "Expected output cannot be empty." }),
  isSample: z.boolean(),
});

const formSchema = z.object({
  courseId: z.string().min(1, { message: "Course ID cannot be empty." }),
  problemId: z.string().min(1, { message: "Problem ID cannot be empty." }),
  name: z.string().min(1, { message: "Name cannot be empty." }),
  problemDescription: z
    .string()
    .min(1, { message: "Description cannot be empty." }),
  requiredPercentageToPass: z.number().min(1).max(100),
  mainMethodBodiesList: z
    .array(problemSolutionsSchema)
    .min(1, { message: "This problem requires at least one solution." }),
  //TODO: finish expected output and stdIn
  expectedOutputAndStdIn: z
    .array(expectedOutputAndStdInSchema)
    .min(1, { message: "Expected outputs or StdIns cannot be empty." }),
});

export type ProblemFormSchemaType = z.infer<typeof formSchema>;

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
      mainMethodBodiesList: [],
      expectedOutputAndStdIn: [],
    },
  });

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    console.log(formData);
  }

  const { data, error } = useGetMyCreatedCoursesQuery();
  const { update, remove } = useFieldArray({
    control: form.control,
    name: "mainMethodBodiesList",
  });

  const dispatch = useAppDispatch();
  useEffect(() => {
    return () => {
      dispatch(clearSolutionDialogData());
    };
  }, [dispatch]);

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">Create Problem</CardTitle>
          <CardDescription>Create a new Problem</CardDescription>
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
                      <Textarea
                        {...field}
                        className="h-28"
                        placeholder="TODO... use a text editing component here with better styling capabilities"
                      />
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
                          field.onChange(vals[0]);
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
                      <ProblemInfoDialog
                        //TODO: maybe look into this on how to use this field value by mapping over the fields generated from the updateFn to make the validation i did simpler
                        // => https://react-hook-form.com/docs/usefieldarray //scroll down to example and maybe select nested form?
                        problemSolutions={field.value}
                        inputErrors={
                          form.getFieldState("mainMethodBodiesList").error as
                            | ProblemSolutionsRHFFieldErrors[]
                            | undefined
                        }
                        inputIsInvalid={
                          !!form.getFieldState(
                            "mainMethodBodiesList",
                            form.formState,
                          ).error
                        }
                        update={update}
                        remove={remove}
                        triggerMainFormFn={form.trigger}
                      />
                    </FormControl>
                    <FormMessage />
                    {!!form.getFieldState(
                      "mainMethodBodiesList",
                      form.formState, //the second parameters indicates that we subscribe to the form state
                    ).error && (
                      <p className="text-sm text-destructive">
                        One or more validation errors occurred
                      </p>
                    )}
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="expectedOutputAndStdIn"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>expectedOutputAndStdIn</FormLabel>
                    <Button
                      type="button"
                      onClick={() =>
                        console.log(form.getValues("expectedOutputAndStdIn"))
                      }
                    >
                      Check expectedOutputAndStdIn array
                    </Button>
                    <FormControl>
                      <ExpectedOutputsStdinsDialog
                        parentField={field}
                        inputIsInvalid={
                          !!form.getFieldState(
                            "expectedOutputAndStdIn",
                            form.formState,
                          ).error
                        }
                        parentFormValidated={form.formState.isSubmitted}
                        setExpectedOutputAndStdIn={form.setValue}
                        formTriggerValidationFn={form.trigger}
                      />
                    </FormControl>
                    <FormMessage />
                    {!!form.getFieldState(
                      "expectedOutputAndStdIn",
                      form.formState,
                    ).error && (
                      <p className="text-sm text-destructive">
                        One or more validation errors occurred
                      </p>
                    )}
                  </FormItem>
                )}
              />

              <Button className="w-full" disabled={form.formState.isSubmitting}>
                {form.formState.isSubmitting ? "Submitting" : "Create problem"}
              </Button>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
