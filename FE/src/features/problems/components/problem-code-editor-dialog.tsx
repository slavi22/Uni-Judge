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
import { Textarea } from "@/components/ui/textarea.tsx";
import { useState } from "react";
import ProblemSolutionDialog from "@/features/problems/components/problem-solution-dialog.tsx";
import { useGetAllProgrammingLanguagesQuery } from "@/features/problems/api/problems-api.ts";
import { v4 as uuidv4 } from "uuid";
import { type SolutionComponentProps } from "@/features/problems/types/problems-types.ts";

type ProblemCodeEditorDialogProps = {
  value: string;
  onChange: (...event: never[]) => void;
  inputIsInvalid: boolean;
};

export default function ProblemCodeEditorDialog({
  value,
  onChange,
  inputIsInvalid,
}: ProblemCodeEditorDialogProps) {
  const { open } = useSidebar();

  const { data } = useGetAllProgrammingLanguagesQuery();
  const [solutions, setSolutions] = useState<SolutionComponentProps[]>([]);

  function handleClick() {
    setSolutions((prevState) => [
      ...prevState,
      { id: uuidv4(), pickedLanguageId: null },
    ]);
    //console.log(solutions);
  }

  function handleSolutionDeletion(id: string) {
    setSolutions(prevState => prevState.filter(item => item.id !== id))
  }


  return (
    <Dialog>
      <DialogTrigger
        asChild
        className={inputIsInvalid ? "border border-destructive" : undefined}
      >
        <Button>Open mainMethodBodiesList Modal</Button>
      </DialogTrigger>
      <DialogContent className={open ? "md:ml-[125px]" : undefined}>
        <DialogHeader>
          <DialogTitle>Add mainMethodBodies</DialogTitle>
          <DialogDescription>Test</DialogDescription>
          {/*TODO: finish*/}
          <Button type="button" onClick={handleClick}>
            Add a solution
          </Button>
          {solutions.map((solution) => (
            <ProblemSolutionDialog
              key={solution.id}
              id={solution.id}
              languages={data!}
              solutions={solutions}
              setSolution={setSolutions}
              handleSolutionDeletion={handleSolutionDeletion}
            />
          ))}
          <Button type="button" onClick={() => console.log(solutions)}>
            Click
          </Button>
          {/*TODO: move or remove*/}
          <Textarea
            value={value}
            onChange={onChange}
            className={
              inputIsInvalid
                ? "border border-destructive focus-visible:border-destructive focus-visible:ring-destructive/50"
                : undefined
            }
          />
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
}
