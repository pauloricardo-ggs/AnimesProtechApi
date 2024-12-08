namespace Domain.Interfaces;

public interface IRequestLogger
{
    void SaveRequest(string requestTypeId, string url, object? requestData, object responseData, int? httpCode = null);
}