import { Button } from "@/components/ui/button.tsx";
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs.tsx";
import TabCardContent from "@/features/code-editor/components/tab-card-content.tsx";
import {
  ResizableHandle,
  ResizablePanel,
  ResizablePanelGroup,
} from "@/components/ui/resizable";
import CodeEditorSelect from "@/features/code-editor/components/code-editor-select.tsx";
import { ProblemInfoDto } from "@/features/problems/types/problems-types.ts";

type CodeEditorProblemProps = {
  data: ProblemInfoDto | undefined;
};

export default function CodeEditorProblem({ data }: CodeEditorProblemProps) {
  return (
    <div className="flex h-full">
      <ResizablePanelGroup direction="horizontal">
        <ResizablePanel defaultSize={25}>
          <Tabs defaultValue="description" className="h-full pl-3">
            <TabsList>
              <TabsTrigger value="description">Problem Description</TabsTrigger>
              <TabsTrigger value="previousSolutions">
                Previous Solutions
              </TabsTrigger>
            </TabsList>
            <TabsContent value="description">
              <TabCardContent title={data?.name} description={data?.description} />
            </TabsContent>
            <TabsContent value="previousSolutions">
              {/*TODO: make an rtk query endpoint which will call the user's previous submissions*/}
              <div>Previous solutions list</div>
            </TabsContent>
          </Tabs>
        </ResizablePanel>
        <ResizableHandle />
        <ResizablePanel defaultSize={75} className="w-0">
          <ResizablePanelGroup direction="vertical">
            <ResizablePanel defaultSize={85} className="mb-3">
              <div className="w-full h-full px-3">
                <CodeEditorSelect availableLanguages={data?.availableLanguages}/>
              </div>
            </ResizablePanel>
            <ResizableHandle withHandle />
            <ResizablePanel defaultSize={15}>
              <div className="flex gap-3 justify-end me-10 mt-3">
                <Button>Test</Button>
                <Button>Submit</Button>
              </div>
            </ResizablePanel>
          </ResizablePanelGroup>
        </ResizablePanel>
      </ResizablePanelGroup>
    </div>
  );
}
