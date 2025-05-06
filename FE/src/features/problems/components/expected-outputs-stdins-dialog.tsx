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
import { ArrowRight } from "lucide-react";
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

type ExpectedOutputsStdinsDialogProps = {
  expectedOutputsAndStdins: ExpectedOutputAndStdin[];
  inputIsInvalid: boolean;
  parentFormValidated: boolean;
  setFormValue: UseFormSetValue<ProblemFormSchemaType>;
  formTriggerValidationFn: UseFormTrigger<ProblemFormSchemaType>;
};

const expectedOutputAndStdInSchema = z.object({
  stdInParam: z.string().min(1, { message: "StdIn cannot be empty." }),
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
}: ExpectedOutputsStdinsDialogProps) {
  const { open } = useSidebar();
  //console.log(parentField);
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      expectedOutputAndStdIn: expectedOutputsAndStdins,
    },
  });

  const { fields, append } = useFieldArray({
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
    const stdInList = expectedOutputAndStdin.map(
      ({ stdInParam }) => stdInParam,
    );
    //TODO: maybe refactor the BE and make it a single object instead of two separate ones
    setFormValue("expectedOutputList", expectedOutputList);
    setFormValue("stdInList", stdInList);
  }

  useEffect(() => {
    if (!openModal && parentFormValidated) {
      formTriggerValidationFn("expectedOutputList");
    }
  }, [formTriggerValidationFn, openModal, parentFormValidated]);

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
            onClick={() =>
              append({
                stdInParam: "",
                expectedOutput: "",
                isSample: false,
              })
            }
          >
            Add new input and output
          </Button>
          <div className="flex flex-col gap-3">
            {/*<Button onClick={() => console.log(form.getValues())}>Log</Button>
            <Button
              onClick={() =>
                setExpectedOutputAndStdIn(
                  "expectedOutputAndStdIn",
                  form.getValues("expectedOutputAndStdIn"),
                )
              }
            >
              Simulate form close to check form values
            </Button>*/}
            <Button>Log</Button>
            {fields.map((formField, index) => (
              <Card key={formField.id} className="py-0">
                {/*<p>{item.stdInParam}</p>
                <p>{item.expectedOutput}</p>
                <p>{item.isSample ? "true" : "false"}</p>*/}
                <CardContent className="flex items-center justify-center gap-5">
                  <Form {...form}>
                    <form
                      onSubmit={form.handleSubmit(onSubmit)}
                      className="flex items-center min-h-32 gap-3"
                    >
                      <FormField
                        control={form.control}
                        name={`expectedOutputAndStdIn.${index}.stdInParam`}
                        render={({ field }) => (
                          <FormItem className="flex flex-col gap-2">
                            <FormControl>
                              <Input placeholder="StdIn parameter" {...field} />
                            </FormControl>
                            <FormMessage className="h-0" />
                            {/*<Button
                              type="button"
                              onClick={() => console.log(field)}
                            >
                              Click
                            </Button>*/}
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
                    </form>
                  </Form>
                  {/*<Input
                    {...field}
                    value={item.stdInParam}
                    onChange={(event) => {
                      const newValue = [...field.value];
                      newValue[index] = {
                        ...newValue[index],
                        stdInParam: event.target.value,
                      };
                      field.onChange(newValue);
                    }}
                    placeholder="StdIn parameter"
                  />
                  <ArrowRight size={128} />
                  <Input
                    {...field}
                    value={item.expectedOutput}
                    onChange={(event) => {
                      const newValue = [...field.value];
                      newValue[index] = {
                        ...newValue[index],
                        expectedOutput: event.target.value,
                      };
                      field.onChange(newValue);
                    }}
                    placeholder="Expected Output"
                  />
                  <div className="flex items-center gap-2 whitespace-nowrap">
                    <Checkbox
                      checked={item.isSample}
                      onCheckedChange={(event) => {
                        const newValue = [...field.value];
                        newValue[index] = {
                          ...newValue[index],
                          isSample: event as boolean,
                        };
                        field.onChange(newValue);
                      }}
                    />
                    <p>Is sample?</p>
                  </div>*/}
                </CardContent>
              </Card>
            ))}
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
