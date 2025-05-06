import { Editor, loader } from "@monaco-editor/react";
import * as monaco from "monaco-editor";
import useTheme from "@/features/theme/hooks/use-theme.ts";
import { registerCsharpProvider } from "@/features/code-editor/utils/monaco-code-editor-intellisense-config.ts";
import { CODE_EDITOR_TEMPLATES } from "@/utils/constants/code-editor-templates.ts";
import { useEffect, useState } from "react";

type MonacoCodeEditorProps = {
  selectedLanguage?: string;
};

export default function MonacoCodeEditor({
  selectedLanguage = "csharp",
}: MonacoCodeEditorProps) {
  const { theme } = useTheme();
  const [editorDisposeFn, setEditorDisposeFn] =
    useState<monaco.IDisposable | null>(null);

  const options: monaco.editor.IStandaloneEditorConstructionOptions = {
    readOnly: false,
    minimap: { enabled: false },
    padding: { top: 16, bottom: 16 },
    scrollBeyondLastLine: false,
  };

  useEffect(() => {
    //https://stackoverflow.com/questions/76660010/duplicate-suggestion-in-monaco-editor-react-next-js
    return () => {
      if (editorDisposeFn && typeof editorDisposeFn.dispose === "function") {
        editorDisposeFn?.dispose();
      }
    };
  }, [editorDisposeFn]);

  return (
    <div className="p-3 rounded">
      <Editor
        className="h-[90vh]"
        language={selectedLanguage}
        defaultValue={CODE_EDITOR_TEMPLATES[selectedLanguage]}
        options={options}
        theme="vs-dark"
        onMount={() => {
          if (selectedLanguage === "csharp") {
            loader.init().then((monaco) => {
              const IDisposable = registerCsharpProvider(monaco);
              setEditorDisposeFn(IDisposable);
            });
          }
        }}
        //theme={theme === "dark" ? "light" : "vs-dark"}
      />
    </div>
  );
}
