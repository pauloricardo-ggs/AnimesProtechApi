using Domain.Constants;
using Domain.Interfaces;
using Newtonsoft.Json;
using Serilog;

namespace Application.Shared;

public class RequestLogger : IRequestLogger
{
    public void SaveRequest(string requestTypeId, string url, object? requestData, object responseData, int? httpCode = null)
    {
        Log.ForContext(LoggerConstant.PROPERTY_SAVE_LOG, LoggerConstant.PROPERTY_SAVE_LOG)
            .ForContext(LoggerConstant.PROPERTY_REQUEST_ID, Guid.NewGuid())
            .ForContext(LoggerConstant.PROPERTY_URL, url)
            .ForContext(LoggerConstant.PROPERTY_REQUEST_DATA, JsonConvert.SerializeObject(requestData))
            .ForContext(LoggerConstant.PROPERTY_RESPONSE_DATA, JsonConvert.SerializeObject(responseData))
            .ForContext(LoggerConstant.PROPERTY_LOG_DATE, DateTime.Now)
            .ForContext(LoggerConstant.PROPERTY_HTTP_CODE, httpCode)
            .ForContext(LoggerConstant.PROPERTY_REQUEST_TYPE_ID, new Guid(requestTypeId))
            .Information("");
    }
}