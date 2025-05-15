import { baseApi } from "@/stores/base-api.ts";
import {
  ClientSubmissionDto,
  ClientSubmissionTestDto,
  ProblemUserSubmissionsDto,
  TestSubmissionBatchResultResponseDto,
  UserSubmissionResultDto,
} from "@/features/submissions/types/submissions-types.ts";
import { toast } from "sonner";

const submissionsApi = baseApi.injectEndpoints({
  endpoints: (build) => ({
    createSubmission: build.mutation<
      UserSubmissionResultDto,
      ClientSubmissionDto
    >({
      query: (dto) => ({
        url: `submissions/create-submission/${dto.courseId}`,
        method: "POST",
        body: dto,
      }),
      async onQueryStarted(_args, {queryFulfilled}) {
        const result = await queryFulfilled;
        if (result.data.isError) {
          toast.error("One or more compilation errors occurred.");
        }
      }
    }),
    createTestSubmission: build.mutation<
      TestSubmissionBatchResultResponseDto[],
      ClientSubmissionTestDto
    >({
      query: (dto) => ({
        url: `submissions/test-submission/${dto.courseId}`,
        method: "POST",
        body: dto,
      }),
    }),
    getAllUserSubmissionsForProblem: build.query<
      ProblemUserSubmissionsDto[],
      { courseId: string; problemId: string }
    >({
      query: ({ courseId, problemId }) =>
        `submissions/get-problem-submissions/${courseId}/${problemId}`,
    }),
  }),
});

export const {
  useCreateSubmissionMutation,
  useCreateTestSubmissionMutation,
  useGetAllUserSubmissionsForProblemQuery,
} = submissionsApi;
