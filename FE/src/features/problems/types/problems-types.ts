export type ClientProblemDto = {
  courseId: string;
  problemId: string;
  name: string;
  description: string;
  requiredPercentageToPass: number;
  mainMethodBodiesList: MainMethodBodyDto[];
  expectedOutputList: ExpectedOutputListDto[];
  stdInList: string[];
  languagesList: LanguagesEnum[];
};

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

export type SolutionComponentProps = {
  id: string;
  pickedLanguageId: number | null;
};
