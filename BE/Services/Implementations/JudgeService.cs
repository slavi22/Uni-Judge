using System.Text;
using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;
using BE.Exceptions;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;
using BE.Util.SubmissionTemplates;
using Newtonsoft.Json;

namespace BE.Services.Implementations;

public class JudgeService : IJudgeService
{
    private readonly IMainMethodBodiesRepository _mainMethodBodiesRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public JudgeService(IMainMethodBodiesRepository mainMethodBodiesRepository, IProblemRepository problemRepository,
        ICourseRepository courseRepository,
        IHttpClientFactory httpClientFactory)
    {
        _mainMethodBodiesRepository = mainMethodBodiesRepository;
        _problemRepository = problemRepository;
        _httpClientFactory = httpClientFactory;
        _courseRepository = courseRepository;
    }

    // this method is used to send a batch of submissions to the judge BE and return the status of each submissionModel (used as test cases foreach test in the problem)
    public async Task<List<SubmissionBatchResultResponseDto>> CreateBatchSubmissions(
        ClientSubmissionDto clientSubmissionDto)
    {
        // Prepare the submissions, send them to the judge backend, poll for their status, and return the results
        var preparedSubmissionsWithResult = await PrepareSubmissionBatchDtoWithResult(clientSubmissionDto);
        var preparedSubmissionsWithoutResult = PrepareSubmissionBatchDtoWithoutResult(preparedSubmissionsWithResult);

        var httpClient = _httpClientFactory.CreateClient("Judge");
        using var content =
            new StringContent(JsonConvert.SerializeObject(preparedSubmissionsWithoutResult), Encoding.UTF8,
                "application/json");

        var judgeRequest = await httpClient.PostAsync("submissions/batch?base64_encoded=true", content);
        var judgeResponse =
            JsonConvert.DeserializeObject<List<SubmissionResponseTokenDto>>(
                await judgeRequest.Content.ReadAsStringAsync());
        // poll the judge BE for the status of each submission
        var submissionStatuses = await PollSubmissionStatuses(judgeResponse);
        // call the ConstructFinalResult method to construct the final result
        var result = ConstructFinalResult(judgeResponse, submissionStatuses, preparedSubmissionsWithResult);
        return result;
    }

    private List<SubmissionBatchResultResponseDto> ConstructFinalResult(
        List<SubmissionResponseTokenDto> judgeResponse, SubmissionResultDto submissionStatuses,
        BatchSubmissionRequestDto preparedSubmissionsWithResult)
    {
        // create a list of SubmissionBatchResultResponseDto to return the results
        var result = new List<SubmissionBatchResultResponseDto>();
        // loop through the response and check if the submission is correct or not
        for (int i = 0; i < judgeResponse.Count; i++)
        {
            var answer = submissionStatuses.Submissions[i].Stdout.Trim().Split("\n").Last();
            if (submissionStatuses.Submissions[i].Status.Id == 3 &&
                (answer == preparedSubmissionsWithResult.Submissions[i].ExpectedOutput ||
                 answer == preparedSubmissionsWithResult.Submissions[i].HiddenExpectedOutput))
            {
                result.Add(new SubmissionBatchResultResponseDto
                {
                    IsCorrect = true,
                    Token = judgeResponse[i].Token,
                    Stdout = string.Join("\n", submissionStatuses.Submissions[i].Stdout.Trim().Split("\n").SkipLast(1)),
                    Status = submissionStatuses.Submissions[i].Status,
                    ExpectedOutput = preparedSubmissionsWithResult.Submissions[i].ExpectedOutput,
                    HiddenExpectedOutput = preparedSubmissionsWithResult.Submissions[i].HiddenExpectedOutput,
                    Stderr = submissionStatuses.Submissions[i].Stderr
                });
            }

            else
            {
                result.Add(new SubmissionBatchResultResponseDto
                {
                    IsCorrect = false,
                    Token = judgeResponse[i].Token,
                    Stdout = string.Join("\n", submissionStatuses.Submissions[i].Stdout.Trim().Split("\n").SkipLast(1)),
                    Status = submissionStatuses.Submissions[i].Status,
                    ExpectedOutput = preparedSubmissionsWithResult.Submissions[i].ExpectedOutput,
                    HiddenExpectedOutput = preparedSubmissionsWithResult.Submissions[i].HiddenExpectedOutput,
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
        if (await _courseRepository.GetCourseByIdAsync(clientSubmissionDto.CourseId) == null)
        {
            throw new CourseNotFoundException($"Course with id '{clientSubmissionDto.CourseId}' was not found.");
        }

        if (await _problemRepository.GetProblemByProblemIdAsync(clientSubmissionDto.ProblemId) == null)
        {
            throw new ProblemNotFoundException($"Problem with id '{clientSubmissionDto.ProblemId}' was not found.");
        }

        // Prepare the submissions by constructing the solution class body and return the prepared submissions

        var preparedSubmissions = new BatchSubmissionRequestDto();

        var problemEntity = await _problemRepository.GetProblemByProblemIdAsync(clientSubmissionDto.ProblemId);

        var mainMethodBodyEntity =
            await _mainMethodBodiesRepository.GetMainMethodBodyByIdAsync(problemEntity.Id);
        // switch case to construct the solution class body based on the language
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
            // index calculated to get the correct stdin for the submissionModel
            int stdInIndex = i * problemEntity.StdInList.Count / problemEntity.ExpectedOutputList.Count;
            string stdIn = problemEntity.StdInList[stdInIndex].StdIn;

            var submissionForRequest = new SubmissionRequestDto
            {
                LanguageId = clientSubmissionDto.LanguageId,
                SourceCode = clientSubmissionDto.SourceCode,
                StdIn = stdIn,
                ExpectedOutput =
                    problemEntity.ExpectedOutputList[i].IsSample // check if the expected output is sample or not
                        ? problemEntity.ExpectedOutputList[i].ExpectedOutput
                        : null,
                HiddenExpectedOutput =
                    !problemEntity.ExpectedOutputList[i].IsSample // check if the expected output is hidden or not
                        ? problemEntity.ExpectedOutputList[i].ExpectedOutput
                        : null
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


    // this method is used to poll the judge BE for the status of each submissionModel periodically (in this context every 1.5 seconds)
    private async Task<SubmissionResultDto> PollSubmissionStatuses(List<SubmissionResponseTokenDto> tokens)
    {
        // Create a new HttpClient, send requests to the judge backend to get the status of each submissionModel, and return the results
        var httpClient = _httpClientFactory.CreateClient("Judge");
        SubmissionResultDto submissionList;
        // loop until all the submissions are accepted (not in queue)
        do
        {
            var newSubmissions = await httpClient.GetAsync(
                $"submissions/batch?tokens={string.Join(",", tokens.Select(x => x.Token))}&base64_encoded=true&fields=status,stdout,stderr,compile_output");
            submissionList =
                JsonConvert.DeserializeObject<SubmissionResultDto>(await newSubmissions.Content.ReadAsStringAsync());
            // wait for 1.5 seconds before polling again (according to the judge BE documentation as it also uses 1.5 seconds as default)
            await Task.Delay(1500);
        } while (submissionList.Submissions.TrueForAll(x => x.Status.Id == 1 || x.Status.Id == 2));

        return submissionList;
    }
}