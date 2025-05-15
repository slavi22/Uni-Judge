export type ClientProblemDto = {
  courseId: string;
  problemId: string;
  name: string;
  description: string;
  requiredPercentageToPass: number;
  mainMethodBodiesList: MainMethodBodyDto[];
  expectedOutputList: ExpectedOutputListDto[];
  stdInList: StdInListDto[];
  languagesList: LanguagesEnum[];
};

export type StdInListDto = {
  isSample: boolean;
  stdIn: string;
}

export type MainMethodBodyDto = {
  languageId: LanguagesEnum;
  solutionTemplate: string;
  mainMethodBodyContent: string;
};

export type ExpectedOutputListDto = {
  isSample: boolean;
  expectedOutput: string;
};

export enum LanguagesEnum {
  Csharp = 51,
  JavaScript = 63,
}

export type CreatedProblemDto = {
  problemId: string;
  courseId: string;
  name: string;
  description: string;
  requiredPercentageToPass: number;
};

export type LanguageDto = {
  languageId: number;
  name: string;
};

export type ProblemSolutionsState = {
  usedLanguages: number[];
}

export type SetUsedLanguagesPayload = {
  oldLanguageId: number;
  newLanguageId: number;
}

export type ProblemSolutions = {
  languageId: string;
  solutionTemplate: string;
  mainMethodBodyContent: string;
};


export type ExpectedOutputAndStdin = {
  expectedOutput: string;
  isSample: boolean;
};

export type ProblemInfoDto = {
  courseId: string;
  problemId: string;
  name: string;
  description: string;
  solutionTemplates: SolutionTemplate[];
  expectedOutputList: string[];
  stdInList: string[];
  availableLanguages: LanguagesEnum[];
}

export type SolutionTemplate = {
  languageId: string;
  solutionTemplateContent: string;
}
