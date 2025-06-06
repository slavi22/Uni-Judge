import { Editor, loader } from "@monaco-editor/react";
import * as monaco from "monaco-editor";
import { registerCsharpProvider } from "@/utils/configs/monaco-code-editor-intellisense-config.ts";
import { CODE_EDITOR_TEMPLATES } from "@/utils/constants/code-editor-templates.ts";
import { useEffect, useState } from "react";
import InfoTooltip from "@/components/tooltips/info-tooltip.tsx";
import { Info } from "lucide-react";
import { cn } from "@/lib/utils.ts";

type MonacoCodeEditorProps = {
  selectedLanguage?: string;
  editorIsForSolutionTemplate: boolean;
  shouldLoadIntellisense: boolean;
  value: string;
  onChange: (newValue: string) => void;
  className?: string;
  scrollBeyondLastLine: boolean;
  editorIsUsedForProblem: boolean;
};

export default function MonacoCodeEditor({
  selectedLanguage = "",
  editorIsForSolutionTemplate,
  shouldLoadIntellisense,
  value,
  onChange,
  className,
  scrollBeyondLastLine,
  editorIsUsedForProblem,
}: MonacoCodeEditorProps) {
  const [editorDisposeFn, setEditorDisposeFn] =
    useState<monaco.IDisposable | null>(null);
  const [editorLanguage, setEditorLanguage] = useState<string | null>(
    selectedLanguage,
  );

  const options: monaco.editor.IStandaloneEditorConstructionOptions = {
    readOnly: false,
    minimap: { enabled: false },
    padding: { top: 16, bottom: 16 },
    scrollBeyondLastLine: scrollBeyondLastLine,
  };

  useEffect(() => {
    //https://stackoverflow.com/questions/76660010/duplicate-suggestion-in-monaco-editor-react-next-js
    return () => {
      if (editorDisposeFn && typeof editorDisposeFn.dispose === "function") {
        monaco.editor.getModels().forEach((model) => model.dispose());
        editorDisposeFn?.dispose();
      }
    };
  }, [editorDisposeFn]);

  useEffect(() => {
    if (editorLanguage !== selectedLanguage && !editorIsUsedForProblem) {
      onChange(
        editorIsForSolutionTemplate
          ? CODE_EDITOR_TEMPLATES[selectedLanguage].solutionTemplate
          : CODE_EDITOR_TEMPLATES[selectedLanguage].mainMethodBodyContent,
      );
      setEditorLanguage(selectedLanguage);
    }
  }, [
    editorIsForSolutionTemplate,
    editorIsUsedForProblem,
    editorLanguage,
    onChange,
    selectedLanguage,
  ]);

  return selectedLanguage ? (
    <div className="p-3 rounded flex flex-col gap-2 overflow-auto w-full">
      <InfoTooltip
        icon={Info}
        tooltipContent="It is recommended to test the code entered below in an IDE first."
        className="ms-auto"
      />
      <Editor
        className={cn("min-h-96", className)}
        language={CODE_EDITOR_TEMPLATES[selectedLanguage]?.langName}
        defaultValue={
          editorIsForSolutionTemplate
            ? CODE_EDITOR_TEMPLATES[selectedLanguage]?.solutionTemplate
            : CODE_EDITOR_TEMPLATES[selectedLanguage]?.mainMethodBodyContent
        }
        value={value}
        onChange={(value) => onChange(value!)}
        options={options}
        theme="vs-dark"
        onMount={() => {
          // this isnt working atm because of the monaco react library's bug causing language providers to not be removed when the language is changed + the editors themselves dont get disposed
          loader.init().then((monaco) => {
            if (selectedLanguage === "51" && shouldLoadIntellisense) {
              const IDisposable = registerCsharpProvider(monaco);
              setEditorDisposeFn(IDisposable!);
            }
          });
        }}
        //theme={theme === "dark" ? "light" : "vs-dark"}
      />
    </div>
  ) : (
    <p className="text-sm text-destructive">Please select a language</p>
  );
}
