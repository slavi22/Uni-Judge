import {
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
import { useState } from "react";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select.tsx";
import { type LanguageDto, type SolutionComponentProps } from "@/features/problems/types/problems-types.ts";
import { Textarea } from "@/components/ui/textarea.tsx";
import { Label } from "@/components/ui/label.tsx";

type ProblemSolutionDialogProps = {
  id: string;
  languages: LanguageDto[];
  solutions: SolutionComponentProps[];
  setSolution: React.Dispatch<React.SetStateAction<SolutionComponentProps[]>>;
  handleSolutionDeletion: (id: string) => void;
};

export default function ProblemSolutionDialog({
  id,
  languages,
  solutions,
  setSolution,
  handleSolutionDeletion,
}: ProblemSolutionDialogProps) {
  const { open } = useSidebar();
  const [selectedLanguage, setSelectedLanguage] = useState<string | undefined>(
    undefined,
  );

  /*console.log(languages);
     console.log(availableLanguages);*/

  function handleSelect(languageId: string) {
    //console.log(selectedLanguage);
    setSelectedLanguage(languageId);
    setSolution((prevState) =>
      prevState.map((item) =>
        item.id === id
          ? {
              ...item,
              pickedLanguageId: Number(languageId),
            }
          : item,
      ),
    );
  }

  //const currentProblem = solutions.find((item) => item.id === id)!;
  const otherProblems = solutions.filter((item) => item.id !== id)!;
  const usedLanguagesIds = otherProblems.map((item) => item.pickedLanguageId!);
  //console.log(otherProblems);

  return (
    <Dialog /*onOpenChange={(open) => open && console.log("fff")}*/>
      <DialogTrigger asChild>
        <Button>
          Edit solution{" "}
          {selectedLanguage &&
            ` - ${
              languages.find(
                (language) =>
                  language.languageId.toString() === selectedLanguage,
              )?.name
            }`}
        </Button>
      </DialogTrigger>
      <DialogContent
        className={
          open
            ? "md:ml-[125px] !max-w-full w-[35%] h-[95%]"
            : "!max-w-full w-[35%] h-[95%]"
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
                      language.languageId.toString() === selectedLanguage,
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
        <div className="mt-3 flex flex-col gap-5 h-screen">
          <Label>Select a language</Label>
          <Select value={selectedLanguage ?? ""} onValueChange={handleSelect}>
            <SelectTrigger className="w-full">
              <SelectValue placeholder="Select a language" />
            </SelectTrigger>
            <SelectContent>
              <SelectGroup>
                <SelectLabel>Programming Languages</SelectLabel>
                {languages
                  .filter((item) => !usedLanguagesIds.includes(item.languageId))
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
          {/*TODO: Monaco components below*/}
          <Label>Solution Template</Label>
          <Textarea
            className="h-1/6"
            placeholder="Monaco editor here for solution template..."
          />
          <Label>Main Method Body Content</Label>
          <Textarea
            className="h-1/6"
            placeholder="Monaco editor here for main method body content..."
          />
          <DialogClose asChild>
            <Button>Save</Button>
          </DialogClose>
          <Button type="button" variant="destructive" onClick={() => handleSolutionDeletion(id)}>
            Delete Solution
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
}
