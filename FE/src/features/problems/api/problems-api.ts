import { baseApi } from "@/stores/base-api.ts";
import { toast } from "sonner";
import {
  ClientProblemDto,
  CreatedProblemDto,
  LanguageDto,
  ProblemInfoDto,
} from "@/features/problems/types/problems-types.ts";

const problemsApi = baseApi.injectEndpoints({
  endpoints: (build) => ({
    createNewProblem: build.mutation<CreatedProblemDto, ClientProblemDto>({
      query: (newProblemData) => ({
        url: "problems/create-problem",
        method: "POST",
        body: newProblemData,
      }),
      async onQueryStarted(_args, { queryFulfilled }) {
        try {
          await queryFulfilled;
          toast.success("Problem created successfully.");
        } catch {
          toast.error("Error creating problem.");
        }
      },
    }),
    //TODO: maybe move this to its respective feature ?
    getAllProgrammingLanguages: build.query<LanguageDto[], void>({
      query: () => "languages/get-all-languages",
    }),
    getProblemInfo: build.query<
      ProblemInfoDto,
      { courseId: string; problemId: string }
    >({
      query: ({ courseId, problemId }) =>
        `problems/get-problem-info/${courseId}/${problemId}`,
    }),
  }),
});

export const {
  useCreateNewProblemMutation,
  useGetAllProgrammingLanguagesQuery,
  useGetProblemInfoQuery,
} = problemsApi;
