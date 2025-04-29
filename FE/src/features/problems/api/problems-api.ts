import { baseApi } from "@/stores/base-api.ts";
import { toast } from "sonner";
import type { ClientProblemDto, CreatedProblemDto, LanguageDto } from "@/features/problems/types/problems-types.ts";

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
    getAllProgrammingLanguages: build.query<LanguageDto[], void>({
      query: () => "languages/get-all-languages"
    })
  }),
});

export const { useCreateNewProblemMutation, useGetAllProgrammingLanguagesQuery } = problemsApi;
