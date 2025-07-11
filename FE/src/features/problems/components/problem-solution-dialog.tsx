﻿import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button.tsx";
import { useSidebar } from "@/components/ui/sidebar.tsx";
import { useEffect, useState } from "react";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select.tsx";
import type {
  LanguageDto,
  ProblemSolutions,
} from "@/features/problems/types/problems-types.ts";
import {
  FieldError,
  useForm,
  UseFormSetValue,
  UseFormTrigger,
} from "react-hook-form";
import { z } from "zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form.tsx";
import { zodResolver } from "@hookform/resolvers/zod";
import { useAppDispatch, useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import {
  removeLanguage,
  setUsedLanguages,
} from "@/features/problems/stores/problem-solutions-slice.ts";
import type { MainMethodBodyList } from "@/features/problems/components/problem-info-dialog.tsx";
import MonacoCodeEditor from "@/components/code-editor/monaco-code-editor.tsx";

type ProblemSolutionDialogProps = {
  solution: ProblemSolutions;
  index: number;
  languages: LanguageDto[];
  setSolution: UseFormSetValue<MainMethodBodyList>;
  deleteSolution: () => void;
  parentFormIsInvalid: boolean;
  problemValidationErrors: FieldError | undefined;
  triggerInfoDialogValidation: UseFormTrigger<MainMethodBodyList>;
};

const formSchema = z.object({
  languageId: z.string().min(1, { message: "Language cannot be empty." }),
  solutionTemplate: z
    .string()
    .min(1, { message: "Solution template cannot be empty." }),
  mainMethodBodyContent: z
    .string()
    .min(1, { message: "Main method body content cannot be empty." }),
});

export default function ProblemSolutionDialog({
  solution,
  index,
  languages,
  setSolution,
  deleteSolution,
  parentFormIsInvalid,
  problemValidationErrors,
  triggerInfoDialogValidation,
}: ProblemSolutionDialogProps) {
  const { open } = useSidebar();
  const [selectedLanguage, setSelectedLanguage] = useState<string | undefined>(
    solution.languageId,
  );
  const dispatch = useAppDispatch();
  const { usedLanguages } = useAppSelector((state) => state.problemSolutions);
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      languageId: solution?.languageId || "",
      solutionTemplate: solution?.solutionTemplate || "",
      mainMethodBodyContent: solution?.mainMethodBodyContent || "",
    },
  });

  /**
   * Handles language selection in the dropdown
   * Updates local state and propagates changes to parent component
   * */
  function handleSelect(zodFn: (value: string) => void, languageId: string) {
    zodFn(languageId);
    setSelectedLanguage(languageId);
    dispatch(
      setUsedLanguages({
        oldLanguageId: Number(selectedLanguage),
        newLanguageId: Number(languageId),
      }),
    );
  }

  /**
   * Handles dialog close event
   * Saves current form state to Redux store and updates parent form
   * Triggers validation on parent form if needed
   */
  function handleDialogClose() {
    setSolution(`mainMethodBodiesList.${index}`, form.getValues());
    if (!shouldApplyInvalidStyling) {
      //debounce to not show the error the first time we close the dialog
      setTimeout(() => {
        setShouldApplyInvalidStyling(true);
      }, 200);
    } else {
      triggerInfoDialogValidation();
    }
  }


  function handleSolutionDeletion() {
    deleteSolution();
    dispatch(removeLanguage(Number(selectedLanguage)));
  }

  const usedLanguagesIds = usedLanguages.map((item) => item);

  // here we check set the state based on the parent form validation and if we have a problem validation error in this current problem, if we do we apply a red border to the dialog trigger button
  const [shouldApplyInvalidStyling, setShouldApplyInvalidStyling] = useState(
    parentFormIsInvalid && !!problemValidationErrors,
  );

  useEffect(() => {
    // if the dialog is opened once and the parent form is invalid, then trigger the validation
    if (shouldApplyInvalidStyling) {
      // programmatically trigger the form submissions so i dont need to manually call trigger() which would break the onChange event in the inputs
      // => https://stackoverflow.com/a/76091816
      // => https://www.react-hook-form.com/api/useform/handlesubmit/
      form.handleSubmit(() => {})();
    }
  }, [shouldApplyInvalidStyling, form, problemValidationErrors]);

  return (
    <Dialog onOpenChange={(open) => !open && handleDialogClose()}>
      <DialogTrigger asChild>
        <div className="flex flex-col gap-2">
          <Button
            variant="outline"
            type="button"
            className={
              parentFormIsInvalid && problemValidationErrors
                ? "border !border-destructive"
                : undefined
            }
          >
            Edit solution{" "}
            {selectedLanguage &&
              ` - ${
                languages.find(
                  (language) =>
                    language.languageId.toString() === selectedLanguage,
                )?.name
              }`}
          </Button>
          {problemValidationErrors &&
            Object.entries(problemValidationErrors).map(([key, value]) => (
              <p key={key} className="text-sm text-destructive">
                {(value as FieldError).message}
              </p>
            ))}
        </div>
      </DialogTrigger>
      <DialogContent
        className={
          open
            ? "md:ml-[125px] !max-w-full w-[35%] h-[95%] overflow-auto"
            : "!max-w-full w-[35%] h-[95%] overflow-auto"
        }
      >
        <DialogHeader>
          <DialogTitle>
            Edit solution for the current problem in{" "}
            {selectedLanguage ? (
              <u>
                {
                  languages.find(
                    (language) =>
                      language.languageId === Number(selectedLanguage),
                  )?.name
                }
              </u>
            ) : (
              <u>Pick a language</u>
            )}
          </DialogTitle>
          <DialogDescription>
            Select a programming language and write the solution template and
            final solution code
          </DialogDescription>
        </DialogHeader>
        <div className="mt-3 flex flex-col gap-5 h-screen mb-16">
          <Form {...form}>
            <form
              onSubmit={(e) => {
                // on why i need this (when i press submit here it propagates to the other react-hook-forms) =>
                // => https://github.com/react-hook-form/react-hook-form/issues/1005#issuecomment-1012188940
                // also here => https://stackoverflow.com/a/78552646
                e.stopPropagation();
                form.handleSubmit(() => {})(e);
              }}
              className="flex flex-col gap-5"
            >
              <FormField
                control={form.control}
                name="languageId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Language</FormLabel>
                    <FormControl>
                      <Select
                        value={field.value}
                        onValueChange={(newValue) =>
                          handleSelect(field.onChange, newValue)
                        }
                      >
                        <SelectTrigger className="w-full">
                          <SelectValue placeholder="Select a language" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectGroup>
                            <SelectLabel>Programming Languages</SelectLabel>
                            {languages
                              .filter(
                                (item) =>
                                  item.languageId.toString() ===
                                    selectedLanguage ||
                                  !usedLanguagesIds.includes(
                                    Number(item.languageId),
                                  ),
                              )
                              .map((item) => (
                                //TODO: maybe sort based on the selected item???
                                <SelectItem
                                  key={item.languageId}
                                  value={item.languageId.toString()}
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
                name="solutionTemplate"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Solution Template</FormLabel>
                    <FormControl>
                      <MonacoCodeEditor
                        key={field.name}
                        selectedLanguage={selectedLanguage}
                        editorIsForSolutionTemplate
                        shouldLoadIntellisense={false}
                        value={field.value}
                        onChange={field.onChange}
                        scrollBeyondLastLine={false}
                        editorIsUsedForProblem={false}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="mainMethodBodyContent"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Main method body content</FormLabel>
                    <FormControl>
                      <MonacoCodeEditor
                        key={field.name}
                        selectedLanguage={selectedLanguage}
                        editorIsForSolutionTemplate={false}
                        shouldLoadIntellisense={false}
                        value={field.value}
                        onChange={field.onChange}
                        scrollBeyondLastLine={false}
                        editorIsUsedForProblem={false}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <DialogClose asChild>
                <Button>Save</Button>
              </DialogClose>
            </form>
          </Form>
          <Button
            type="button"
            variant="destructive"
            onClick={handleSolutionDeletion}
          >
            Delete Solution
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
}
