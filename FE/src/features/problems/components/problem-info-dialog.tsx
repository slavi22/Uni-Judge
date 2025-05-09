import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button.tsx";
import { useSidebar } from "@/components/ui/sidebar.tsx";
import ProblemSolutionDialog from "@/features/problems/components/problem-solution-dialog.tsx";
import { useGetAllProgrammingLanguagesQuery } from "@/features/problems/api/problems-api.ts";
import type {
  ProblemSolutions,
} from "@/features/problems/types/problems-types.ts";
import {
  useFieldArray,
  useForm,
  type UseFormSetValue,
} from "react-hook-form";
import type { ProblemFormSchemaType } from "@/features/problems/components/create-new-problem-form.tsx";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form.tsx";

type ProblemCodeEditorDialogProps = {
  parentProblemSolutions: ProblemSolutions[];
  setMainMethodBodyContent: UseFormSetValue<ProblemFormSchemaType>;
  parentFormIsInvalid: boolean;
};

const mainMethodBodyContentSchema = z.object({
  languageId: z.string().min(1, { message: "Language is required." }),
  solutionTemplate: z.string().min(1, {
    message: "Solution template cannot be empty.",
  }),
  mainMethodBodyContent: z.string().min(1, {
    message: "Main method body content cannot be empty.",
  }),
});

const formSchema = z.object({
  mainMethodBodiesList: z
    .array(mainMethodBodyContentSchema)
    .min(1, { message: "At least one main method body is required." }),
});

export type MainMethodBodyList = z.infer<typeof formSchema>;

export default function ProblemInfoDialog({
  parentProblemSolutions,
  setMainMethodBodyContent,
  parentFormIsInvalid,
}: ProblemCodeEditorDialogProps) {
  const { open } = useSidebar();

  const { data: languages } = useGetAllProgrammingLanguagesQuery();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: { mainMethodBodiesList: parentProblemSolutions },
  });

  function onSubmit(formData: z.infer<typeof formSchema>) {
    console.log(formData);
  }

  const {
    fields,
    append,
    remove: removeSolution,
  } = useFieldArray({
    control: form.control,
    name: "mainMethodBodiesList",
  });

  return (
    <Dialog
      onOpenChange={() =>
        setMainMethodBodyContent(
          "mainMethodBodiesList",
          form.getValues().mainMethodBodiesList,
        )
      }
    >
      <DialogTrigger
        asChild
        className={
          parentFormIsInvalid ? "border !border-destructive" : undefined
        }
      >
        <Button variant="outline">Open mainMethodBodiesList Modal</Button>
      </DialogTrigger>
      <DialogContent className={open ? "md:ml-[125px]" : undefined}>
        <DialogHeader>
          <DialogTitle>Add mainMethodBodies</DialogTitle>
          <DialogDescription>Test</DialogDescription>
          <Button
            type="button"
            onClick={() =>
              append({
                languageId: "",
                mainMethodBodyContent: "",
                solutionTemplate: "",
              })
            }
          >
            Add a solution
          </Button>
          {fields.length > 0 && (
            <Form {...form}>
              <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-2">
                {fields.map((solution, index) => (
                  <FormField
                    key={solution.id}
                    control={form.control}
                    name={`mainMethodBodiesList.${index}`}
                    render={({ field }) => (
                      <FormItem>
                        <FormControl>
                          <ProblemSolutionDialog
                            solution={field.value}
                            index={index}
                            languages={languages!}
                            setSolution={form.setValue}
                            deleteSolution={() => removeSolution(index)}
                            parentFormIsInvalid={
                              !!form.getFieldState(
                                "mainMethodBodiesList",
                                form.formState,
                              ).error
                            }
                            problemValidationErrors={
                              form.getFieldState(
                                `mainMethodBodiesList.${index}`,
                              ).error
                            }
                            triggerInfoDialogValidation={form.trigger}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                ))}
              </form>
            </Form>
          )}
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
}
