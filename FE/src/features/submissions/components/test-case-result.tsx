import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Card, CardContent } from "@/components/ui/card.tsx";
import { useEffect, useState } from "react";
import { Input } from "@/components/ui/input.tsx";
import { ArrowRight, CircleX, Plus } from "lucide-react";
import { Button } from "@/components/ui/button.tsx";
import {
  type TestCase,
  TestSubmissionBatchResultResponseDto,
} from "@/features/submissions/types/submissions-types.ts";

type TestCaseResultProps = {
  parentTestCases: TestCase[];
  setParentTestCases: (value: TestCase[]) => void;
  testResult?: TestSubmissionBatchResultResponseDto[] | undefined;
};

export default function TestCaseResult({
  parentTestCases,
  setParentTestCases,
  testResult,
}: TestCaseResultProps) {
  const [localTestCases, setLocalTestCases] =
    useState<TestCase[]>(parentTestCases);
  const [selectedOuterTab, setSelectedOuterTab] = useState<string>("testCases");
  const [selectedInnerTab, setSelectedInnerTab] =
    useState<string>("test-case-1");

  function handleAddButtonClick() {
    setLocalTestCases((prevState) => [
      ...prevState,
      { stdIn: "", expectedOutput: "" },
    ]);
  }

  function handleRemoveTestCase(arrIndex: number) {
    setLocalTestCases((prevState) =>
      prevState.filter((_, index) => index !== arrIndex),
    );
    setSelectedInnerTab(`test-case-${arrIndex}`);
  }

  useEffect(() => {
    if (testResult) {
      setSelectedOuterTab("testResult");
    }
  }, [testResult]);

  useEffect(() => {
    setParentTestCases(localTestCases)
  }, [localTestCases, setParentTestCases]);

  return (
    <Tabs
      value={selectedOuterTab}
      onValueChange={(value) => setSelectedOuterTab(value)}
      className="ms-3 min-w-[400px]"
    >
      <TabsList>
        <TabsTrigger value="testCases">Test Cases</TabsTrigger>
        <TabsTrigger value="testResult">Test Result</TabsTrigger>
      </TabsList>
      <TabsContent value="testCases">
        <Card>
          <Tabs
            className="ms-1"
            value={selectedInnerTab}
            onValueChange={(value) => setSelectedInnerTab(value)}
          >
            <TabsList className="flex gap-1 me-1">
              {localTestCases.map((_, index) => (
                <span key={index} className="relative group">
                  <TabsTrigger
                    key={index}
                    value={`test-case-${index + 1}`}
                    className="pr-6"
                  >
                    Test case {index + 1}
                  </TabsTrigger>
                  {selectedInnerTab === `test-case-${index + 1}` && (
                    <button
                      className="absolute text-red-600 cursor-pointer top-0 right-0 opacity-0 group-hover:opacity-100 transition-opacity z-10"
                      onClick={() => handleRemoveTestCase(index)}
                    >
                      <CircleX className="w-4 h-4" />
                    </button>
                  )}
                </span>
              ))}
              <Button
                variant="outline"
                size="sm"
                onClick={handleAddButtonClick}
              >
                <Plus />
              </Button>
            </TabsList>
            {localTestCases.map((testCase, index) => (
              <TabsContent key={index} value={`test-case-${index + 1}`}>
                <div className="flex w-full gap-1 items-center">
                  <span className="w-1/2">
                    <p className="ms-1">stdin=</p>
                    <Input
                      value={testCase.stdIn || ""}
                      onChange={(e) => {
                        const newStdInValue = e.target.value;
                        setLocalTestCases((prevState) =>
                          prevState.map((item, i) =>
                            i === index
                              ? {
                                  ...item,
                                  stdIn: newStdInValue,
                                }
                              : item,
                          ),
                        );
                      }}
                      placeholder="Stdin parameter"
                    />
                  </span>
                  <ArrowRight size={28} className="mt-7 mb-auto" />
                  <span className="me-1 grow">
                    <p className="ms-1">expected output=</p>
                    <Input
                      value={testCase.expectedOutput || ""}
                      onChange={(e) => {
                        const newExpectedOutputValue = e.target.value;
                        setLocalTestCases((prevState) =>
                          prevState.map((item, i) =>
                            i === index
                              ? {
                                  ...item,
                                  expectedOutput: newExpectedOutputValue,
                                }
                              : item,
                          ),
                        );
                      }}
                      placeholder="Expected output"
                    />
                  </span>
                </div>
              </TabsContent>
            ))}
          </Tabs>
        </Card>
      </TabsContent>
      <TabsContent value="testResult">
        <Card>
          {testResult?.map((item, index) => (
            <CardContent key={index} className="flex gap-3">
              {item.isCorrect ? "✅" : "❌"}
              <p>StdIn: {item.stdIn};</p>
              <p>Expected Output: {item.expectedOutput}</p>
            </CardContent>
          ))}
        </Card>
      </TabsContent>
    </Tabs>
  );
}
