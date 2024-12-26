using System.Text;
using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;
using BE.Repositories;
using BE.Util.SubmissionTemplates;
using Newtonsoft.Json;

namespace BE.Services;

public class JudgeService : IJudgeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public JudgeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // this method is used to send a batch of submissions to the judge BE and return the status of each submission
    public async Task<List<SubmissionBatchResultResponse>> AddBatchSubmissions(SubmissionBatchDto submissions)
    {
        // prepare the submissions by constructing the solution class body
        var preparedSubmissions = PrepareSubmissionBatchDto(submissions);
        // create a new httpclient that will send requests to the judge BE
        var httpClient = _httpClientFactory.CreateClient("Judge");
        // serialize the submissions to json and send them to the judge BE
        using var content =
            new StringContent(JsonConvert.SerializeObject(preparedSubmissions), Encoding.UTF8, "application/json");
        var request = await httpClient.PostAsync("submissions/batch?base64_encoded=true", content);
        // receive a response which contains a list of the tokens of the submissions (since the submissions are still being processed)
        // deserialize the response to a list of SubmissionResponseTokenDto
        var response =
            JsonConvert.DeserializeObject<List<SubmissionResponseTokenDto>>(await request.Content.ReadAsStringAsync());
        // poll the judge BE for the status of each submission
        var submissionStatuses = await PollSubmissionStatus(response);
        // create a list of SubmissionBatchResponseTokenStatusDto which contains the token and the status of each submission
        var result = new List<SubmissionBatchResultResponse>();
        // add the token and the status of each submission to the list
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

        // return the list of SubmissionBatchResponseTokenStatusDto
        return result;
    }

    // this method is used to prepare the submissions by constructing the solution class body
    private static SubmissionBatchDto PrepareSubmissionBatchDto(SubmissionBatchDto submissions)
    {
        foreach (var submission in submissions.Submissions)
        {
            // first parameter in the method should be gotten from the db
            submission.SourceCode = CSharpTemplate.ConstructSolution("Test", submission.SourceCode, submission.StdIn);
        }

        return submissions;
    }

    // this method is used to poll the judge BE for the status of each submission periodically (in this context every 1.5 seconds)
    private async Task<SubmissionResultDto> PollSubmissionStatus(List<SubmissionResponseTokenDto> response)
    {
        // create a new httpclient that will send requests to the judge BE
        var httpClient = _httpClientFactory.CreateClient("Judge");
        // create SubmissionResultDto which hold a list of SubmissionStatusDto
        SubmissionResultDto submissionList;
        // loop until all the submissions are processed (not in queue)
        do
        {
            var newSubmissions = await httpClient.GetAsync(
                $"submissions/batch?tokens={string.Join(",", response.Select(x => x.Token))}&base64_encoded=true&fields=status,stderr,expected_output");
            // deserialize the response to SubmissionResultDto
            submissionList =
                JsonConvert.DeserializeObject<SubmissionResultDto>(await newSubmissions.Content.ReadAsStringAsync());
            // wait for 1.5 seconds before polling again (according to the judge BE documentation as it also uses 1.5 seconds as default)
            await Task.Delay(1500);
        } while (submissionList.Submissions.TrueForAll(x => x.Status.Id == 1));

        // return the list of SubmissionStatusDto
        return submissionList;
    }
}