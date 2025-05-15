import { Button } from "@/components/ui/button.tsx";
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs.tsx";
import TabCardContent from "@/features/submissions/components/tab-card-content.tsx";
import {
  ResizableHandle,
  ResizablePanel,
  ResizablePanelGroup,
} from "@/components/ui/resizable";
import CodeEditorSelect from "@/features/submissions/components/code-editor-select.tsx";
import { ProblemInfoDto } from "@/features/problems/types/problems-types.ts";
import { useState } from "react";
import {
  useCreateSubmissionMutation,
  useCreateTestSubmissionMutation,
  useGetAllUserSubmissionsForProblemQuery,
} from "@/features/submissions/api/submissions-api.ts";
import TestCaseResult from "@/features/submissions/components/test-case-result.tsx";
import { TestCase } from "@/features/submissions/types/submissions-types.ts";
import PreviousUserSolutions from "@/features/submissions/components/previous-user-solutions.tsx";

type CodeEditorProblemProps = {
  data: ProblemInfoDto | undefined;
  courseId: string | undefined;
  problemId: string | undefined;
};

export default function CodeEditorProblem({
  data,
  courseId,
  problemId,
}: CodeEditorProblemProps) {
  const [editorValue, setEditorValue] = useState<string | undefined>();
  const [languageId, setLanguageId] = useState<string | undefined>();
  const [selectedTab, setSelectedTab] = useState<string>("description");
  const [testCases, setTestCases] = useState<TestCase[]>(
    Array.from({ length: data?.stdInList.length || 0 }).map((_item, index) => ({
      stdIn: data?.stdInList[index],
      expectedOutput: data?.expectedOutputList[index],
    })) as TestCase[],
  );

  const [createSubmission, { isLoading: isCreateSubmissionLoading }] =
    useCreateSubmissionMutation();
  const [
    createTestSubmission,
    { data: testSubmissionData, isLoading: isCreateTestSubmissionLoading },
  ] = useCreateTestSubmissionMutation();
  const { data: userSubmissionsForCurrentProblemData, refetch: refetchAllUserSubmissionsForProblem } =
    useGetAllUserSubmissionsForProblemQuery({
      courseId: courseId!,
      problemId: problemId!,
    });

  async function handleSubmitSolutionButtonClick() {
    await createSubmission({
      courseId: courseId!,
      problemId: problemId!,
      languageId: languageId!,
      sourceCode: editorValue!,
    });
    setSelectedTab("previousSolutions");
    refetchAllUserSubmissionsForProblem();
  }

  async function handleTestButtonClick() {
    await createTestSubmission({
      courseId: courseId!,
      problemId: problemId!,
      languageId: languageId!,
      sourceCode: editorValue!,
      testCases: testCases,
    });
  }

  return (
    <div className="flex h-full">
      <ResizablePanelGroup direction="horizontal">
        <ResizablePanel defaultSize={25}>
          <Tabs
            value={selectedTab}
            className="h-full pl-3"
            onValueChange={(value) => setSelectedTab(value)}
          >
            <TabsList>
              <TabsTrigger value="description">Problem Description</TabsTrigger>
              <TabsTrigger value="previousSolutions">
                Previous Solutions
              </TabsTrigger>
            </TabsList>
            <TabsContent value="description">
              <TabCardContent
                title={data?.name}
                description={data?.description}
                stdInList={data?.stdInList}
                expectedOutputList={data?.expectedOutputList}
              />
              <div className="flex flex-col ms-3 mt-3 me-auto">
                <p>StdInList - {data?.stdInList}</p>
                <p>ExpectedOutputList - {data?.expectedOutputList}</p>
              </div>
            </TabsContent>
            <TabsContent value="previousSolutions">
              {/*TODO: make an rtk query endpoint which will call the user's previous submissions*/}
              <PreviousUserSolutions
                data={userSubmissionsForCurrentProblemData}
              />
            </TabsContent>
          </Tabs>
        </ResizablePanel>
        <ResizableHandle />
        <ResizablePanel defaultSize={75} className="w-0">
          <ResizablePanelGroup direction="vertical">
            <ResizablePanel defaultSize={70} className="mb-3">
              <div className="w-full h-full px-3">
                <CodeEditorSelect
                  availableLanguages={data?.availableLanguages}
                  solutionTemplate={data?.solutionTemplates}
                  setProblemCodeEditorValue={(value: string) =>
                    setEditorValue(value)
                  }
                  setProblemLanguageId={(value: string) => setLanguageId(value)}
                />
              </div>
            </ResizablePanel>
            <ResizableHandle withHandle />
            <ResizablePanel defaultSize={30}>
              <div className="flex w-full mt-3">
                <TestCaseResult
                  parentTestCases={testCases}
                  setParentTestCases={(value: TestCase[]) =>
                    setTestCases(value)
                  }
                  testResult={testSubmissionData}
                />
                <div className="flex gap-3 ms-auto me-10">
                  <Button
                    onClick={handleTestButtonClick}
                    disabled={
                      isCreateSubmissionLoading || isCreateTestSubmissionLoading
                    }
                  >
                    {isCreateTestSubmissionLoading ? "Submitting..." : "Test"}
                  </Button>
                  <Button
                    onClick={handleSubmitSolutionButtonClick}
                    disabled={
                      isCreateSubmissionLoading || isCreateTestSubmissionLoading
                    }
                  >
                    {isCreateSubmissionLoading ? "Submitting..." : "Submit"}
                  </Button>
                </div>
              </div>
            </ResizablePanel>
          </ResizablePanelGroup>
        </ResizablePanel>
      </ResizablePanelGroup>
    </div>
  );
}
