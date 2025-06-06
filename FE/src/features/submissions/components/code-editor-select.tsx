import MonacoCodeEditor from "@/components/code-editor/monaco-code-editor.tsx";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { useEffect, useState } from "react";
import {
  LanguagesEnum,
  type SolutionTemplate,
} from "@/features/problems/types/problems-types.ts";

type CodeEditorSelectProps = {
  availableLanguages: LanguagesEnum[] | undefined;
  solutionTemplate?: SolutionTemplate[] | undefined;
  setProblemCodeEditorValue: (value: string) => void;
  setProblemLanguageId: (value: string) => void;
};

export default function CodeEditorSelect({
  availableLanguages,
  solutionTemplate,
  setProblemCodeEditorValue,
  setProblemLanguageId,
}: CodeEditorSelectProps) {
  const [selectedLanguage, setSelectedLanguage] = useState(
    availableLanguages?.[0].toString(),
  );
  const templateIndex = solutionTemplate?.findIndex(
    (solution) => solution.languageId === selectedLanguage,
  );
  const [codeEditorValue, setCodeEditorValue] = useState(
    solutionTemplate?.[templateIndex!]?.solutionTemplateContent,
  );

  function handleLanguageChange(value: string) {
    setProblemLanguageId(value);
    setSelectedLanguage(value);
    const newTemplateIndex = solutionTemplate?.findIndex(
      (solution) => solution.languageId === value,
    );
    setCodeEditorValue(
      solutionTemplate?.[newTemplateIndex!]?.solutionTemplateContent,
    );
  }

  function handleEditorBlur() {
    setProblemCodeEditorValue(codeEditorValue!);
  }

  useEffect(() => {
    if (selectedLanguage !== undefined && codeEditorValue !== undefined) {
      setProblemLanguageId(selectedLanguage);
      setProblemCodeEditorValue(codeEditorValue);
    }
  }, [codeEditorValue, selectedLanguage, setProblemCodeEditorValue, setProblemLanguageId]);

  return (
    <div onBlur={handleEditorBlur}>
      <Select
        onValueChange={(value) => handleLanguageChange(value)}
        defaultValue={selectedLanguage}
      >
        <SelectTrigger className="w-62 mt-1">
          <SelectValue placeholder="Select a programming language" />
        </SelectTrigger>
        <SelectContent>
          <SelectGroup>
            <SelectLabel>Select a programming language</SelectLabel>
            {availableLanguages?.map((item) => (
              <SelectItem key={item} value={item.toString()}>
                {LanguagesEnum[item.valueOf()] === "Csharp"
                  ? "C#"
                  : LanguagesEnum[item.valueOf()]}
              </SelectItem>
            ))}
          </SelectGroup>
        </SelectContent>
      </Select>
      <MonacoCodeEditor
        selectedLanguage={selectedLanguage}
        editorIsForSolutionTemplate={false}
        shouldLoadIntellisense={false} // TODO: the monaco-react library is bugged and is causing memory leaks because it doesnt remove old instances of the old editor even when calling dispose. currently this will stay disabled maybe look if they fixed it in the future
        value={codeEditorValue!}
        onChange={setCodeEditorValue}
        className="h-[80vh]"
        scrollBeyondLastLine={true}
        editorIsUsedForProblem={true}
      />
    </div>
  );
}
