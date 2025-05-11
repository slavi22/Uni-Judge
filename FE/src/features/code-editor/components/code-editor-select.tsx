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
import { useState } from "react";
import {
  type LanguagesEnum,
} from "@/features/problems/types/problems-types.ts";

type CodeEditorSelectProps = {
  availableLanguages: LanguagesEnum[] | undefined;
};

export default function CodeEditorSelect({
  availableLanguages,
}: CodeEditorSelectProps) {
  const [selectedLanguage, setSelectedLanguage] = useState(
    availableLanguages?.[0].toString(),
  );
  const [codeEditorValue, setCodeEditorValue] = useState("");
  //TODO: fix intellisense when switching to js
  return (
    <>
      <Select
        onValueChange={(value) => setSelectedLanguage(value)}
        defaultValue={selectedLanguage}
      >
        <SelectTrigger className="w-62 mt-1">
          <SelectValue placeholder="Select a programming language" />
        </SelectTrigger>
        <SelectContent>
          <SelectGroup>
            <SelectLabel>Select a programming language</SelectLabel>
            <SelectItem value="51">C#</SelectItem>
            <SelectItem value="63">JavaScript</SelectItem>
          </SelectGroup>
        </SelectContent>
      </Select>
      <MonacoCodeEditor
        key={selectedLanguage}
        selectedLanguage={selectedLanguage}
        editorIsForSolutionTemplate={false}
        shouldLoadIntellisense={true}
        value={codeEditorValue}
        onChange={setCodeEditorValue}
        className="h-[80vh]"
        scrollBeyondLastLine={true}
      />
    </>
  );
}
