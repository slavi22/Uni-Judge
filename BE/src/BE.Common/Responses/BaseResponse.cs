namespace BE.Common.Responses;

public abstract class BaseResponse
{
    public string Title { get; set; }
    public int Status {get; set;}
    public string Detail { get; set; }
}