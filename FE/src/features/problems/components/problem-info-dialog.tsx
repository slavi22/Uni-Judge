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
import { useState } from "react";
import ProblemSolutionDialog from "@/features/problems/components/problem-solution-dialog.tsx";
import { useGetAllProgrammingLanguagesQuery } from "@/features/problems/api/problems-api.ts";
import { v4 as uuidv4 } from "uuid";
import type {
  ProblemSolutions,
  ProblemSolutionsRHFFieldErrors,
  SolutionComponentProps,
} from "@/features/problems/types/problems-types.ts";
import { useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import type { UseFieldArrayRemove } from "react-hook-form";

type ProblemCodeEditorDialogProps = {
  problemSolutions: ProblemSolutions[];
  inputErrors: ProblemSolutionsRHFFieldErrors[] | undefined;
  inputIsInvalid: boolean;
  update: (index: number, data: ProblemSolutions) => void;
  remove: UseFieldArrayRemove;
  triggerMainFormFn: () => void;
};

export default function ProblemInfoDialog({
  inputErrors,
  inputIsInvalid,
  update,
  remove,
  triggerMainFormFn,
}: ProblemCodeEditorDialogProps) {
  const { open } = useSidebar();

  const { data } = useGetAllProgrammingLanguagesQuery();
  const [solutions, setSolutions] = useState<SolutionComponentProps[]>([]);
  const { solutionDialogData } = useAppSelector(
    (state) => state.problemSolutions,
  );

  /**
   * Handles the addition of a new solution
   * This function updates the state of the solutions and adds (using RHF's form update fn) a new solution to the form schema "mainMethodBodiesList" array
   * */
  function handleClick() {
    setSolutions((prevState) => [
      ...prevState,
      { id: uuidv4(), pickedLanguageId: null },
    ]);
    update(solutions.length, {
      languageId: 0,
      solutionTemplate: "",
      mainMethodBodyContent: "",
    });
  }

  /**
   * Handles the deletion of a solution
   * This function updates the state of the solutions and removes the solution from the form schema "mainMethodBodiesList" array
   * */
  function handleSolutionDeletion(id: string, removeIndex: number) {
    setSolutions((prevState) => prevState.filter((item) => item.id !== id));
    remove(removeIndex);
  }

  return (
    <Dialog>
      <DialogTrigger
        asChild
        className={inputIsInvalid ? "border !border-destructive" : undefined}
      >
        <Button variant="outline">Open mainMethodBodiesList Modal</Button>
      </DialogTrigger>
      <DialogContent className={open ? "md:ml-[125px]" : undefined}>
        <DialogHeader>
          <DialogTitle>Add mainMethodBodies</DialogTitle>
          <DialogDescription>Test</DialogDescription>
          <Button type="button" onClick={handleClick}>
            Add a solution
          </Button>
          {solutions.map((solution, index) => (
            <ProblemSolutionDialog
              key={solution.id}
              id={solution.id}
              languages={data!}
              solutions={solutions}
              setSolution={setSolutions}
              handleSolutionDeletion={handleSolutionDeletion}
              solutionDialogData={
                solutionDialogData ? solutionDialogData[solution.id] : undefined
              }
              rhfUpdateFn={update}
              updateFnIndex={index}
              parentFormIsInvalid={inputIsInvalid}
              problemValidationError={
                inputErrors ? inputErrors[index] : undefined
              }
              triggerMainFormFn={triggerMainFormFn}
            />
          ))}
          <Button type="button" onClick={() => console.log(solutions)}>
            Click
          </Button>
          {/*TODO: move or remove*/}
          {/*<Textarea
            value={value}
            onChange={onChange}
            className={
              inputIsInvalid
                ? "border border-destructive focus-visible:border-destructive focus-visible:ring-destructive/50"
                : undefined
            }
          />*/}
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
}
