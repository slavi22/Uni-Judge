export type ClientSubmissionDto = {
  courseId: string;
  problemId: string;
  languageId: string;
  sourceCode: string;
};

export type UserSubmissionResultDto = {
  submissionId: string;
  isError: boolean;
  testCases: TestCaseDto[];
};

export type TestCaseDto = {
  isCorrect: boolean;
  expectedOutput?: string;
  compileOutput?: string;
  stdOut?: string;
  stdErr?: string;
  status: TestCaseStatusDto;
};

export type TestCaseStatusDto = {
  id: number;
  description: string;
};

export type TestCase = {
  stdIn: string;
  expectedOutput: string;
};

export type ClientSubmissionTestDto = {
  courseId: string;
  problemId: string;
  languageId: string;
  sourceCode: string;
  testCases: ClientTestStdInsAndExpectedOutput[];
};

export type ClientTestStdInsAndExpectedOutput = {
  stdIn: string;
  expectedOutput: string;
};

export type TestSubmissionBatchResultResponseDto = {
  isCorrect: boolean;
  expectedOutput?: string;
  stdIn?: string;
  stdOut?: string;
  status: TestCaseStatusDto;
  compileOutput?: string;
  stdErr?: string;
};

export type ProblemUserSubmissionsDto = {
  submissionId: string;
  isError?: boolean;
  errorResult?: string;
  isPassing: boolean;
  languageId: string;
};
