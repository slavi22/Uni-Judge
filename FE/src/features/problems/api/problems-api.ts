import { baseApi } from "@/stores/base-api.ts";
import { toast } from "sonner";
import {
  ClientProblemDto,
  CreatedProblemDto,
  EditProblemInfo,
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

    getEditProblemInfo: build.query<
      EditProblemInfo,
      { courseId: string; problemId: string }
    >({
      query: ({ courseId, problemId }) =>
        `problems/get-edit-problem-info/${courseId}/${problemId}`, //TODO: this endpoint doesnt return everything so fix
    }),
    editProblem: build.mutation<
      CreatedProblemDto,
      { courseId: string; problemId: string; data: ClientProblemDto }
    >({
      query: ({ courseId, problemId, data }) => ({
        url: `problems/edit-problem/${courseId}/${problemId}`,
        method: "PUT",
        body: data,
      }),
      async onQueryStarted(_args, { queryFulfilled }) {
        try {
          await queryFulfilled;
          toast.success("Problem edited successfully.");
        } catch {
          toast.error("Error editing problem.");
        }
      },
    }),
  }),
});

export const {
  useCreateNewProblemMutation,
  useGetAllProgrammingLanguagesQuery,
  useGetProblemInfoQuery,
  useGetEditProblemInfoQuery,
  useEditProblemMutation,
} = problemsApi;
