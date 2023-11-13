namespace Alerting.Functions.Management.Domain.Models;
public class AlertFailure
{
    public AlertFailure(string alertTypeName, string message, string statusCode, string errorDetails)
    {
        Id = Guid.NewGuid();
        AlertTypeName = alertTypeName;
        Message = message;
        StatusCode = statusCode;
        ErrorDetails = errorDetails;
    }

    public Guid Id { get; set; }
    public string AlertTypeName { get; set; }
    public string StatusCode { get; set; }
    public string ErrorDetails { get; set; }
    public string Message { get; set; }
}
