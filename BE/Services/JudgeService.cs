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

        var preparedSubmissions = await PrepareSubmissionBatchDto(batchSubmissions);
        var httpClient = _httpClientFactory.CreateClient("Judge");
        using var content =
            new StringContent(JsonConvert.SerializeObject(preparedSubmissions), Encoding.UTF8, "application/json");
        var request = await httpClient.PostAsync("submissions/batch?base64_encoded=true", content);
        var response =
            JsonConvert.DeserializeObject<List<SubmissionResponseTokenDto>>(await request.Content.ReadAsStringAsync());
        var submissionStatuses = await PollSubmissionStatus(response);
        var result = new List<SubmissionBatchResultResponse>();
        for (int i = 0; i < response.Count; i++)
        {
            result.Add(new SubmissionBatchResultResponse
            {
                Token = response[i].Token,
                Status = submissionStatuses.Submissions[i].Status.Description,
                ExpectedOutput = submissionStatuses.Submissions[i].ExpectedOutput,
                Stderr = submissionStatuses.Submissions[i].Stderr
            });
        }

        return result;
    }

    // this method is used to prepare the submissions by constructing the solution class body
    private async Task<BatchSubmissionRequestDto> PrepareSubmissionBatchDto(BatchSubmissionDto batchSubmissions)
    {
        // Prepare the submissions by constructing the solution class body and return the prepared submissions

        var preparedSubmissions = new BatchSubmissionRequestDto();
        foreach (var submission in batchSubmissions.Submissions)
        {
            var mainMethodBodyEntity =
                await _dbContext.MainMethodBodies.FirstOrDefaultAsync(x => x.ProblemId == submission.ProblemId);
            var problemEntity = await _dbContext.Problems.FirstOrDefaultAsync(x => x.Id == submission.ProblemId);
            switch (submission.LanguageId)
            {
                case "51":
                {
                    submission.SourceCode = CSharpTemplate.ConstructSolutionBase(mainMethodBodyEntity.MainMethodBodyContent,
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
                    StdIn = problemEntity.StdInList?[i],
                    ExpectedOutput = problemEntity.ExpectedOutputList[i]
                };

                preparedSubmissions.Submissions.Add(submissionForRequest);
            }

        }

        //Console.WriteLine(JsonConvert.SerializeObject(preparedSubmissions));
        // serialize it
        //Console.WriteLine(JsonConvert.SerializeObject(preparedSubmissions));
        return preparedSubmissions;
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
                $"submissions/batch?tokens={string.Join(",", response.Select(x => x.Token))}&base64_encoded=true&fields=status,stderr,expected_output");
            submissionList =
                JsonConvert.DeserializeObject<SubmissionResultDto>(await newSubmissions.Content.ReadAsStringAsync());
            // wait for 1.5 seconds before polling again (according to the judge BE documentation as it also uses 1.5 seconds as default)
            await Task.Delay(1500);
        } while (submissionList.Submissions.TrueForAll(x => x.Status.Id == 1));

        return submissionList;
    }
}