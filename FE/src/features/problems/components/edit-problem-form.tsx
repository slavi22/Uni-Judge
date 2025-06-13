import { z } from "zod";
import { type ComponentProps, useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  useEditProblemMutation,
} from "@/features/problems/api/problems-api.ts";
import { useNavigate } from "react-router";
import { useGetMyCreatedCoursesQuery } from "@/features/courses/api/course-api.ts";
import { useAppDispatch, useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import {
  clearUsedLanguages,
  editProblemSetUsedLanguages,
} from "@/features/problems/stores/problem-solutions-slice.ts";
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
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select.tsx";
import { Input } from "@/components/ui/input.tsx";
import { Textarea } from "@/components/ui/textarea.tsx";
import { Slider } from "@/components/ui/slider.tsx";
import { Button } from "@/components/ui/button.tsx";
import ProblemInfoDialog from "@/features/problems/components/problem-info-dialog.tsx";
import ExpectedOutputsStdinsDialog from "@/features/problems/components/expected-outputs-stdins-dialog.tsx";
import { type EditProblemInfo } from "@/features/problems/types/problems-types.ts";

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

export type EditFormStdInSchema = z.infer<typeof stdInSchema>;

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

type EditProblemFormProps = {
  courseId: string | undefined;
  problemId: string | undefined;
  data?: EditProblemInfo | undefined;
} & ComponentProps<"div">;

export default function EditProblemForm({
  data,
  courseId,
  problemId,
  className,
  ...props
}: EditProblemFormProps) {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      courseId: data?.courseId,
      problemId: data?.problemId,
      name: data?.name,
      description: data?.description,
      requiredPercentageToPass: data?.requiredPercentageToPass,
      mainMethodBodiesList: data?.mainMethodBodiesList.map((item) => ({
        ...item,
        languageId: item.languageId.toString(),
      })),
      expectedOutputList: data?.expectedOutputList,
      stdInList: data?.stdInList,
      languagesList: data?.languagesList,
    },
  });

  const [editProblem] = useEditProblemMutation();
  const navigate = useNavigate();

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    const reformattedFormData = {
      ...formData,
      mainMethodBodiesList: formData.mainMethodBodiesList.map((item) => ({
        ...item,
        languageId: Number(item.languageId),
      })),
    };

    const result = await editProblem({
      courseId: courseId!,
      problemId: problemId!,
      data: reformattedFormData,
    });
    if (!result.error) {
      navigate("/courses"); //TODO: navigate to my created problems page if i decide to create one
    }
  }

  const { data: myCreatedCoursesData, error } = useGetMyCreatedCoursesQuery();
  const { usedLanguages } = useAppSelector((state) => state.problemSolutions);

  const dispatch = useAppDispatch();

  useEffect(() => {
    return () => {
      dispatch(clearUsedLanguages());
    };
  }, [dispatch]);

  useEffect(() => {
    if (!usedLanguages.length) {
      dispatch(editProblemSetUsedLanguages(form.getValues("languagesList")));
    } else {
      form.setValue("languagesList", usedLanguages);
    }
  }, [dispatch, form, usedLanguages]);

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">Edit Problem</CardTitle>
          <CardDescription>Edit the current problem</CardDescription>
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
                        disabled={true}
                      >
                        <SelectTrigger className="w-full">
                          <SelectValue placeholder="Select a course" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectGroup>
                            <SelectLabel>
                              Select a course from the ones you've created
                            </SelectLabel>
                            {myCreatedCoursesData &&
                              myCreatedCoursesData.map((item) => (
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
                    <FormLabel>Edit Solution Templates</FormLabel>
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
                    <FormLabel>Edit Expected Outputs & Inputs</FormLabel>
                    <FormControl>
                      <ExpectedOutputsStdinsDialog
                        expectedOutputsAndStdins={field.value} //TODO: here is the discrepancy
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
                        stdInListFromParent={form.getValues("stdInList")}
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
                {form.formState.isSubmitting ? "Submitting" : "Edit problem"}
              </Button>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
