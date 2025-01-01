using System.Text;
using BE.Data;
using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;
using BE.Repositories;
using BE.Util.SubmissionTemplates;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BE.Services;

public class JudgeService : IJudgeService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;

    public JudgeService(IHttpClientFactory httpClientFactory, AppDbContext dbContext)
    {
        _httpClientFactory = httpClientFactory;
        _dbContext = dbContext;
    }

    // this method is used to send a batch of submissions to the judge BE and return the status of each submission (used as test cases foreach test in the problem)
    public async Task<List<SubmissionBatchResultResponse>> AddBatchSubmissions(BatchSubmissionDto batchSubmissions)
    {
        // Prepare the submissions, send them to the judge backend, poll for their status, and return the results

        var preparedSubmissionsWithResult = await PrepareSubmissionBatchDtoWithResult(batchSubmissions);
        var preparedSubmissionsWithoutResult = PrepareSubmissionBatchDtoWithoutResult(preparedSubmissionsWithResult);

        var httpClient = _httpClientFactory.CreateClient("Judge");
        using var content =
            new StringContent(JsonConvert.SerializeObject(preparedSubmissionsWithoutResult), Encoding.UTF8,
                "application/json");

        var request = await httpClient.PostAsync("submissions/batch?base64_encoded=true", content);
        var response =
            JsonConvert.DeserializeObject<List<SubmissionResponseTokenDto>>(await request.Content.ReadAsStringAsync());
        var submissionStatuses = await PollSubmissionStatus(response);
        var result = new List<SubmissionBatchResultResponse>();
        // TODO: FINISH THIS
        for (int i = 0; i < response.Count; i++)
        {
            var answer = submissionStatuses.Submissions[i].Stdout.Trim().Split("\n").Last();
            if (submissionStatuses.Submissions[i].Status.Id == 3 && answer == preparedSubmissionsWithResult.Submissions[i].ExpectedOutput)
            {
                result.Add(new SubmissionBatchResultResponse
                {
                    IsCorrect = true,
                    Token = response[i].Token,
                    Stdout = string.Join("\n", submissionStatuses.Submissions[i].Stdout.Trim().Split("\n").SkipLast(1)),
                    Status = submissionStatuses.Submissions[i].Status,
                    Stderr = submissionStatuses.Submissions[i].Stderr
                });
            }

            else
            {
                result.Add(new SubmissionBatchResultResponse
                {
                    IsCorrect = false,
                    Token = response[i].Token,
                    Stdout = string.Join("\n", submissionStatuses.Submissions[i].Stdout.Trim().Split("\n").SkipLast(1)),
                    Status = submissionStatuses.Submissions[i].Status,
                    ExpectedOutput = submissionStatuses.Submissions[i].ExpectedOutput,
                    Stderr = submissionStatuses.Submissions[i].Stderr
                });
            }
        }

        return result;
    }

    // this method is used to prepare the submissions by constructing the solution class body
    private async Task<BatchSubmissionRequestDto> PrepareSubmissionBatchDtoWithResult(
        BatchSubmissionDto batchSubmissions)
    {
        // Prepare the submissions by constructing the solution class body and return the prepared submissions

        var preparedSubmissions = new BatchSubmissionRequestDto();
        foreach (var submission in batchSubmissions.Submissions)
        {
            var mainMethodBodyEntity =
                await _dbContext.MainMethodBodies.FirstOrDefaultAsync(x => x.ProblemId == submission.ProblemId);
            var problemEntity = await _dbContext.Problems.Include(problemModel => problemModel.StdInList)
                .Include(problemModel => problemModel.ExpectedOutputList).FirstOrDefaultAsync(x => x.Id == submission.ProblemId);
            switch (submission.LanguageId)
            {
                case "51":
                {
                    submission.SourceCode = CSharpTemplate.ConstructSolutionBase(
                        mainMethodBodyEntity.MainMethodBodyContent,
                        submission.SourceCode);
                    break;
                }
            }

            for (int i = 0; i < problemEntity.ExpectedOutputList.Count; i++)
            {
                var submissionForRequest = new SingleSubmissionRequestDto
                {
                    LanguageId = submission.LanguageId,
                    SourceCode = submission.SourceCode,
                    StdIn = problemEntity.StdInList[i].StdIn,
                    ExpectedOutput = problemEntity.ExpectedOutputList[i].ExpectedOutput
                };

                preparedSubmissions.Submissions.Add(submissionForRequest);
            }
        }

        return preparedSubmissions;
    }

    private BatchSubmissionRequestDto PrepareSubmissionBatchDtoWithoutResult(
        BatchSubmissionRequestDto batchSubmissions)
    {
        // Prepare the submissions by removing the expected output and return the prepared submissions

        var batchSubmissionWithoutExpectedOutput = new BatchSubmissionRequestDto();
        foreach (var submission in batchSubmissions.Submissions)
        {
            var editedSubmission = new SingleSubmissionRequestDto
            {
                LanguageId = submission.LanguageId,
                SourceCode = submission.SourceCode,
                StdIn = submission.StdIn
            };
            batchSubmissionWithoutExpectedOutput.Submissions.Add(editedSubmission);
        }

        return batchSubmissionWithoutExpectedOutput;
    }


    // this method is used to poll the judge BE for the status of each submission periodically (in this context every 1.5 seconds)
    private async Task<SubmissionResultDto> PollSubmissionStatus(List<SubmissionResponseTokenDto> response)
    {
        // Create a new HttpClient, send requests to the judge backend to get the status of each submission, and return the results
        var httpClient = _httpClientFactory.CreateClient("Judge");
        SubmissionResultDto submissionList;
        // loop until all the submissions are processed (not in queue)
        do
        {
            var newSubmissions = await httpClient.GetAsync(
                //TODO: add the message attribute to check if it returns the error output
                $"submissions/batch?tokens={string.Join(",", response.Select(x => x.Token))}&base64_encoded=true&fields=status,stdout,stderr,compile_output,expected_output");
            submissionList =
                JsonConvert.DeserializeObject<SubmissionResultDto>(await newSubmissions.Content.ReadAsStringAsync());
            // wait for 1.5 seconds before polling again (according to the judge BE documentation as it also uses 1.5 seconds as default)
            await Task.Delay(1500);
        } while (submissionList.Submissions.TrueForAll(x => x.Status.Id == 1));

        return submissionList;
    }
}