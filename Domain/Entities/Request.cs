namespace Domain.Entities;

public class Request
{
    public Guid Id { get; private set; }
    public string Url { get; private set; }
    public string RequestData { get; private set; }
    public string ResponseData { get; private set; }
    public DateTime LogDate { get; private set; }
    public int? HttpCode { get; private set; }
    public Guid RequestTypeId { get; private set; }
    public RequestType RequestType { get; private set; }

    // public Request(string url, string requestData, string responseData, int attempt, int? httpCode, DateTime logDate)
    // {
    //     Url = url;
    //     RequestData = requestData;
    //     ResponseData = responseData;
    //     Attempt = attempt;
    //     HttpCode = httpCode;
    //     LogDate = logDate;
    // }
}
