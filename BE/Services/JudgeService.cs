using System.Text;
using BE.DTOs.Judge;
using BE.Repositories;
using Newtonsoft.Json;

namespace BE.Services;

public class JudgeService : IJudgeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public JudgeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task AddBatchSubmissions(SubmissionBatchDto submissions)
    {
        var httpClient = _httpClientFactory.CreateClient("Judge");
        using var content =
            new StringContent(JsonConvert.SerializeObject(submissions), Encoding.UTF8, "application/json");
        var request = await httpClient.PostAsync("submissions/batch?base64_encoded=true", content);
        Console.WriteLine(await request.Content.ReadAsStringAsync());
        var response =
            JsonConvert.DeserializeObject<List<SubmissionResponseTokenDto>>(await request.Content.ReadAsStringAsync());

        await CheckIfSubmissionHasBeenProcessed(response);


    }

    private async Task CheckIfSubmissionHasBeenProcessed(List<SubmissionResponseTokenDto> response)
    {
        var httpClient = _httpClientFactory.CreateClient("Judge");
        HttpResponseMessage newSubmissions;
        SubmissionResultDto submissionList;
        do
        {
            newSubmissions = await httpClient.GetAsync(
                $"submissions/batch?tokens={string.Join(",", response.Select(x => x.Token))}&base64_encoded=true&fields=status");
            submissionList = JsonConvert.DeserializeObject<SubmissionResultDto>(await newSubmissions.Content.ReadAsStringAsync());
            submissionList.Submissions.ForEach(x => Console.WriteLine($"{x.Status.Id} {x.Status.Description}"));
            await Task.Delay(1500);
        } while (submissionList.Submissions.TrueForAll(x => x.Status.Id != 3));

        // TODO: Develop this for further user
        submissionList.Submissions.ForEach(x => Console.WriteLine($"{x.Status.Id} {x.Status.Description}"));
    }
}