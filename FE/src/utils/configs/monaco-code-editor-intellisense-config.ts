import * as monaco from "monaco-editor";
import { BE_URL } from "@/utils/constants/consts.ts";

type Type = "complete" | "signature" | "hover" | "codeCheck";
type Request = {
  Code: string;
  Position: number;
  Assemblies: string[];
};

async function sendRequest(type: Type, request: Request) {
  let endPoint;
  switch (type) {
    case "complete":
      endPoint = "csharp/complete";
      break;
    case "signature":
      endPoint = "csharp/signature";
      break;
    case "hover":
      endPoint = "csharp/hover";
      break;
    case "codeCheck":
      endPoint = "csharp/codeCheck";
      break;
  }
  const response = await fetch(`${BE_URL}code-intellisense/${endPoint}`, {
    method: "POST",
    body: JSON.stringify(request),
  });

  return await response.json();
}

export function registerCsharpProvider(monacoInstance: typeof monaco) {

  const assemblies = ["bin/Debug/net9.0/BE.Presentation.dll"]; //TODO: change to the corresponding dll in prod since currently in the docker container we are still in dev mode

  const IDisposable = monacoInstance.languages.registerCompletionItemProvider("csharp", {
    triggerCharacters: [".", " "],
    provideCompletionItems: async (
      model,
      position,
    ): Promise<monaco.languages.CompletionList | null | undefined> => {
      const suggestions = [];

      const request = {
        Code: model.getValue(),
        Position: model.getOffsetAt(position),
        Assemblies: assemblies,
      };

      const resultQ = await sendRequest("complete", request);

      for (const elem of resultQ) {
        suggestions.push({
          label: {
            label: elem.Suggestion,
            description: elem.Description,
          },
          kind: monacoInstance.languages.CompletionItemKind.Function,
          insertText: elem.Suggestion,
          range: {
            startLineNumber: position.lineNumber,
            endLineNumber: position.lineNumber,
            startColumn: position.column,
            endColumn: position.column,
          },
        });
      }

      return { suggestions: suggestions };
    },
  });

  monacoInstance.languages.registerSignatureHelpProvider("csharp", {
    signatureHelpTriggerCharacters: ["("],
    signatureHelpRetriggerCharacters: [","],

    provideSignatureHelp: async (
      model,
      position,
    ): Promise<monaco.languages.SignatureHelpResult | null | undefined> => {
      const request: Request = {
        Code: model.getValue(),
        Position: model.getOffsetAt(position),
        Assemblies: assemblies,
      };

      const resultQ = await sendRequest("signature", request);
      if (!resultQ) return;

      const signatures = [];
      for (const signature of resultQ.Signatures) {
        const params = [];
        for (const param of signature.Parameters) {
          params.push({
            label: param.Label,
            documentation: param.Documentation ?? "",
          });
        }

        signatures.push({
          label: signature.Label,
          documentation: signature.Documentation ?? "",
          parameters: params,
        });
      }

      const signatureHelp: monaco.languages.SignatureHelp = {
        signatures: signatures,
        activeParameter: resultQ.activeParameter,
        activeSignature: resultQ.activeSignature
      };

      return {
        value: signatureHelp,
        dispose: () => {},
      } as monaco.languages.SignatureHelpResult;
    },
  });

  monacoInstance.languages.registerHoverProvider("csharp", {
    provideHover: async function (model, position) {
      const request: Request = {
        Code: model.getValue(),
        Position: model.getOffsetAt(position),
        Assemblies: assemblies,
      };

      const resultQ = await sendRequest("hover", request);

      if (resultQ) {
        const posStart = model.getPositionAt(resultQ.OffsetFrom);
        const posEnd = model.getPositionAt(resultQ.OffsetTo);

        return {
          range: new monacoInstance.Range(
            posStart.lineNumber,
            posStart.column,
            posEnd.lineNumber,
            posEnd.column,
          ),
          contents: [{ value: resultQ.Information }],
        };
      }

      return null;
    },
  });

  monacoInstance.editor.onDidCreateModel(function (model) {
    async function validate() {
      const request: Request = {
        Code: model.getValue(),
        Assemblies: assemblies,
        Position: 0,
      };

      const resultQ = await sendRequest("codeCheck", request);

      const markers = [];

      for (const elem of resultQ) {
        const posStart = model.getPositionAt(elem.OffsetFrom);
        const posEnd = model.getPositionAt(elem.OffsetTo);
        markers.push({
          severity: elem.Severity,
          startLineNumber: posStart.lineNumber,
          startColumn: posStart.column,
          endLineNumber: posEnd.lineNumber,
          endColumn: posEnd.column,
          message: elem.Message,
          code: elem.Id,
        });
      }

      monacoInstance.editor.setModelMarkers(model, "csharp", markers);
    }

    let handle: string | number | NodeJS.Timeout | undefined = undefined;
    model.onDidChangeContent(() => {
      monacoInstance.editor.setModelMarkers(model, "csharp", []);
      clearTimeout(handle);
      handle = setTimeout(() => validate(), 500);
    });
    validate();
  });

  return IDisposable;
}
