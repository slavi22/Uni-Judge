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
import {
  useFieldArray,
  useForm,
  type UseFormSetValue,
  type UseFormTrigger,
} from "react-hook-form";
import type { ProblemFormSchemaType } from "@/features/problems/components/create-new-problem-form.tsx";
import { Input } from "@/components/ui/input.tsx";
import { Checkbox } from "@/components/ui/checkbox.tsx";
import { ArrowRight, Plus, Trash2 } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card.tsx";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form.tsx";
import { useEffect, useState } from "react";
import { type ExpectedOutputAndStdin } from "@/features/problems/types/problems-types.ts";
import { EditFormStdInSchema } from "@/features/problems/components/edit-problem-form.tsx";

type ExpectedOutputsStdinsDialogProps = {
  expectedOutputsAndStdins: ExpectedOutputAndStdin[];
  inputIsInvalid: boolean;
  parentFormValidated: boolean;
  setFormValue: UseFormSetValue<ProblemFormSchemaType>;
  formTriggerValidationFn: UseFormTrigger<ProblemFormSchemaType>;
  stdInListFromParent?: EditFormStdInSchema[];
};

const stdInSchema = z.object({
  stdIn: z.string().min(1, { message: "StdIn cannot be empty." }),
  isSample: z.boolean(),
});

const expectedOutputAndStdInSchema = z.object({
  stdInParam: z
    .array(stdInSchema)
    .min(1, { message: "At least one stdIn is required." }),
  expectedOutput: z
    .string()
    .min(1, { message: "Expected output cannot be empty." }),
  isSample: z.boolean(),
});

const formSchema = z.object({
  expectedOutputAndStdIn: z.array(expectedOutputAndStdInSchema).min(1, {
    message: "At least one expected output and stdIn pair is required.",
  }),
});

export default function ExpectedOutputsStdinsDialog({
  expectedOutputsAndStdins,
  inputIsInvalid,
  parentFormValidated,
  setFormValue,
  formTriggerValidationFn,
  stdInListFromParent,
}: ExpectedOutputsStdinsDialogProps) {
  const { open } = useSidebar();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      expectedOutputAndStdIn: expectedOutputsAndStdins,
    },
  });

  const { fields, append, remove } = useFieldArray({
    control: form.control,
    name: "expectedOutputAndStdIn",
  });

  function onSubmit(formData: z.infer<typeof formSchema>) {
    console.log(formData);
  }

  const [openModal, setOpenModal] = useState(false);

  function handleDialogOpen() {
    if (parentFormValidated) {
      form.handleSubmit(onSubmit)();
    }
    setOpenModal(!openModal);
    const expectedOutputAndStdin = form.getValues("expectedOutputAndStdIn");
    const expectedOutputList = expectedOutputAndStdin.map(
      ({ expectedOutput, isSample }) => ({ expectedOutput, isSample }),
    );
    const stdInList = expectedOutputAndStdin.flatMap(
      ({ stdInParam, isSample }) =>
        stdInParam.map((item) => ({
          stdIn: item.stdIn,
          isSample: isSample,
        })),
    );
    //TODO: maybe refactor the BE and make it a single object instead of two separate ones
    setFormValue("expectedOutputList", expectedOutputList);
    setFormValue("stdInList", stdInList);
  }

  useEffect(() => {
    if (!openModal && parentFormValidated) {
      formTriggerValidationFn("expectedOutputList");
      formTriggerValidationFn("stdInList");
    }
  }, [formTriggerValidationFn, openModal, parentFormValidated]);

  useEffect(() => {
    if (stdInListFromParent) {
      stdInListFromParent.map((_item, index) =>
        form.setValue(
          `expectedOutputAndStdIn.${index}.stdInParam.0`,
          stdInListFromParent[index],
        ),
      );
    }
  }, [form, stdInListFromParent]);

  return (
    <Dialog open={openModal} onOpenChange={() => handleDialogOpen()}>
      <DialogTrigger asChild>
        <Button
          variant="outline"
          className={inputIsInvalid ? "border !border-destructive" : undefined}
          onClick={() => handleDialogOpen()}
        >
          Edit Expected Outputs & Inputs
        </Button>
      </DialogTrigger>
      <DialogContent className={open ? "md:ml-[125px] !max-w-150" : undefined}>
        <DialogHeader>
          <DialogTitle>Add expected outputs and stdins</DialogTitle>
          <DialogDescription>
            Add your expected outputs and stdins here for the problem.
          </DialogDescription>
        </DialogHeader>
        <div className="flex flex-col gap-3">
          <Button
            onClick={() => {
              append(
                {
                  stdInParam: [{ stdIn: "", isSample: false }],
                  expectedOutput: "",
                  isSample: false,
                },
                {
                  focusName: `expectedOutputAndStdIn.${fields.length}.stdInParam.0.stdIn`,
                },
              );
            }}
          >
            <Plus />
            Add new input and output
          </Button>
          <div className="flex gap-3">
            <Form {...form}>
              <form
                onSubmit={form.handleSubmit(onSubmit)}
                className="flex flex-col items-center gap-3"
              >
                {fields.map((formField, index) => (
                  <Card key={formField.id} className="min-h-32">
                    <CardContent className="flex items-center justify-center gap-5">
                      <FormField
                        control={form.control}
                        name={`expectedOutputAndStdIn.${index}.stdInParam.0.stdIn`}
                        render={({ field }) => (
                          <FormItem className="flex flex-col gap-2">
                            <FormControl>
                              <Input placeholder="StdIn parameter" {...field} />
                            </FormControl>
                            <FormMessage className="h-0" />
                          </FormItem>
                        )}
                      />
                      <ArrowRight size={48} />
                      <FormField
                        control={form.control}
                        name={`expectedOutputAndStdIn.${index}.expectedOutput`}
                        render={({ field }) => (
                          <FormItem className="flex flex-col gap-2">
                            <FormControl>
                              <Input placeholder="Expected output" {...field} />
                            </FormControl>
                            <FormMessage className="h-0" />
                          </FormItem>
                        )}
                      />
                      <FormField
                        control={form.control}
                        name={`expectedOutputAndStdIn.${index}.isSample`}
                        render={({ field }) => (
                          <FormItem className="flex gap-2">
                            <FormControl>
                              <Checkbox
                                checked={field.value}
                                onCheckedChange={field.onChange}
                              />
                            </FormControl>
                            <FormLabel className="text-nowrap">
                              Is sample?
                            </FormLabel>
                          </FormItem>
                        )}
                      />
                      <Button
                        variant="destructive"
                        size="icon"
                        onClick={() => remove(index)}
                      >
                        <Trash2 />
                      </Button>
                    </CardContent>
                  </Card>
                ))}
              </form>
            </Form>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
