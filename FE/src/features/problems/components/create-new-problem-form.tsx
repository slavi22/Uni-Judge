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
import ProblemInfoDialog from "@/features/problems/components/problem-info-dialog.tsx";
import { Textarea } from "@/components/ui/textarea.tsx";
import { Slider } from "@/components/ui/slider.tsx";
import { useAppDispatch, useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import { clearUsedLanguages } from "@/features/problems/stores/problem-solutions-slice.ts";
import ExpectedOutputsStdinsDialog from "@/features/problems/components/expected-outputs-stdins-dialog.tsx";
import { useCreateNewProblemMutation } from "@/features/problems/api/problems-api.ts";
import { useNavigate } from "react-router";

const problemSolutionsSchema = z.object({
  languageId: z.string().min(1, { message: "Language ID cannot be empty." }),
  solutionTemplate: z.string().min(1, {
    message: "Solution template cannot be empty.",
  }),
  mainMethodBodyContent: z.string().min(1, {
    message: "Main method body content cannot be empty.",
  }),
});

const expectedOutputSchema = z.object({
  expectedOutput: z
    .string()
    .min(1, { message: "Expected output cannot be empty." }),
  isSample: z.boolean(),
});

const stdInSchema = z.object({
  stdIn: z.string().min(1, { message: "StdIn cannot be empty." }),
  isSample: z.boolean(),
});

const formSchema = z.object({
  courseId: z.string().min(1, { message: "Course ID cannot be empty." }),
  problemId: z.string().min(1, { message: "Problem ID cannot be empty." }),
  name: z.string().min(1, { message: "Name cannot be empty." }),
  description: z.string().min(1, { message: "Description cannot be empty." }),
  requiredPercentageToPass: z.number().min(1).max(100),
  mainMethodBodiesList: z
    .array(problemSolutionsSchema)
    .min(1, { message: "This problem requires at least one solution." }),
  expectedOutputList: z
    .array(expectedOutputSchema)
    .min(1, { message: "Expected outputs or StdIns cannot be empty." }),
  stdInList: z
    .array(stdInSchema)
    .min(1, { message: "StdIn list cannot be empty." }),
  languagesList: z
    .number()
    .array()
    .min(1, { message: "At least one language is required." }),
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
      description: "",
      requiredPercentageToPass: 50,
      mainMethodBodiesList: [],
      expectedOutputList: [],
      stdInList: [],
      languagesList: [],
    },
  });

  const [createNewProblem] = useCreateNewProblemMutation();
  const navigate = useNavigate();

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    const reformattedFormData = {
      ...formData,
      mainMethodBodiesList: formData.mainMethodBodiesList.map((item) => ({
        ...item,
        languageId: Number(item.languageId),
      })),
    };
    //console.log(reformattedFormData);
    const result = await createNewProblem(reformattedFormData);
    if (!result.error) {
      navigate("/courses"); //TODO: navigate to my created problems page if i decide to create one
    }
  }

  const { data, error } = useGetMyCreatedCoursesQuery();
  const { usedLanguages } = useAppSelector((state) => state.problemSolutions);

  const dispatch = useAppDispatch();
  useEffect(() => {
    return () => {
      dispatch(clearUsedLanguages());
    };
  }, [dispatch]);

  useEffect(() => {
    form.setValue("languagesList", usedLanguages);
  }, [form, usedLanguages]);

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
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Description</FormLabel>
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
                    <FormLabel>Problem Solutions</FormLabel>
                    <FormControl>
                      <ProblemInfoDialog
                        // => https://react-hook-form.com/docs/usefieldarray //scroll down to example and maybe select nested form?
                        parentProblemSolutions={field.value}
                        setMainMethodBodyContent={form.setValue}
                        parentFormIsInvalid={
                          !!form.getFieldState(
                            "mainMethodBodiesList",
                            form.formState,
                          ).error
                        }
                        triggerMainFromFn={form.trigger}
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
                name="expectedOutputList"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Expected Outputs and Standard Inputs</FormLabel>
                    <FormControl>
                      <ExpectedOutputsStdinsDialog
                        expectedOutputsAndStdins={field.value}
                        inputIsInvalid={
                          !!form.getFieldState(
                            "expectedOutputList",
                            form.formState,
                          ).error ||
                          !!form.getFieldState("stdInList", form.formState)
                            .error
                        }
                        parentFormValidated={form.formState.isSubmitted}
                        setFormValue={form.setValue}
                        formTriggerValidationFn={form.trigger}
                      />
                    </FormControl>
                    <FormMessage />
                    {(!!form.getFieldState("expectedOutputList", form.formState)
                      .error ||
                      !!form.getFieldState("stdInList", form.formState)
                        .error) && (
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
