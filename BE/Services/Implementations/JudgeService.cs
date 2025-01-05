using System.Text;
using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;
using BE.Util.SubmissionTemplates;
using Newtonsoft.Json;

namespace BE.Services.Implementations;

public class JudgeService : IJudgeService
{
    //private readonly AppDbContext _dbContext;
    private readonly IMainMethodBodiesRepository _mainMethodBodiesRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public JudgeService(IMainMethodBodiesRepository mainMethodBodiesRepository, IProblemRepository problemRepository, IHttpClientFactory httpClientFactory)
    {
        _mainMethodBodiesRepository = mainMethodBodiesRepository;
        _problemRepository = problemRepository;
        _httpClientFactory = httpClientFactory;
    }

    // this method is used to send a batch of submissions to the judge BE and return the status of each submission (used as test cases foreach test in the problem)
    public async Task<List<SubmissionBatchResultResponseDto>> AddBatchSubmissions(ClientSubmissionDto clientSubmissionDto)
    {
        // Prepare the submissions, send them to the judge backend, poll for their status, and return the results

        var preparedSubmissionsWithResult = await PrepareSubmissionBatchDtoWithResult(clientSubmissionDto);
        var preparedSubmissionsWithoutResult = PrepareSubmissionBatchDtoWithoutResult(preparedSubmissionsWithResult);

        var httpClient = _httpClientFactory.CreateClient("Judge");
        using var content =
            new StringContent(JsonConvert.SerializeObject(preparedSubmissionsWithoutResult), Encoding.UTF8,
                "application/json");

        var request = await httpClient.PostAsync("submissions/batch?base64_encoded=true", content);
        var response =
            JsonConvert.DeserializeObject<List<SubmissionResponseTokenDto>>(await request.Content.ReadAsStringAsync());
        var submissionStatuses = await PollSubmissionStatus(response);
        var result = new List<SubmissionBatchResultResponseDto>();
        // TODO: FINISH THIS
        for (int i = 0; i < response.Count; i++)
        {
            var answer = submissionStatuses.Submissions[i].Stdout.Trim().Split("\n").Last();
            if (submissionStatuses.Submissions[i].Status.Id == 3 && answer == preparedSubmissionsWithResult.Submissions[i].ExpectedOutput)
            {
                result.Add(new SubmissionBatchResultResponseDto
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
                result.Add(new SubmissionBatchResultResponseDto
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
        ClientSubmissionDto clientSubmissionDto)
    {
        // Prepare the submissions by constructing the solution class body and return the prepared submissions

        var preparedSubmissions = new BatchSubmissionRequestDto();

        var mainMethodBodyEntity = await _mainMethodBodiesRepository.GetMainMethodBodyByIdAsync(clientSubmissionDto.ProblemId);
        var problemEntity = await _problemRepository.GetProblemByIdAsync(clientSubmissionDto.ProblemId);
        switch (clientSubmissionDto.LanguageId)
        {
            case "51":
            {
                clientSubmissionDto.SourceCode = CSharpTemplate.ConstructSolutionBase(
                    mainMethodBodyEntity.MainMethodBodyContent,
                    clientSubmissionDto.SourceCode);
                break;
            }
        }

        for (int i = 0; i < problemEntity.ExpectedOutputList.Count; i++)
        {
            var submissionForRequest = new SubmissionRequestDto
            {
                LanguageId = clientSubmissionDto.LanguageId,
                SourceCode = clientSubmissionDto.SourceCode,
                StdIn = problemEntity.StdInList[i].StdIn,
                ExpectedOutput = problemEntity.ExpectedOutputList[i].ExpectedOutput
            };

            preparedSubmissions.Submissions.Add(submissionForRequest);
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
            var editedSubmission = new SubmissionRequestDto
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
                $"submissions/batch?tokens={string.Join(",", response.Select(x => x.Token))}&base64_encoded=true&fields=status,stdout,stderr,compile_output,expected_output");
            submissionList =
                JsonConvert.DeserializeObject<SubmissionResultDto>(await newSubmissions.Content.ReadAsStringAsync());
            // wait for 1.5 seconds before polling again (according to the judge BE documentation as it also uses 1.5 seconds as default)
            await Task.Delay(1500);
        } while (submissionList.Submissions.TrueForAll(x => x.Status.Id == 1));

        return submissionList;
    }
}