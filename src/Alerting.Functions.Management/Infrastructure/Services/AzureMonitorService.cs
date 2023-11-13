namespace Alerting.Functions.Opsgenie.Infrastructure.Services;

public interface IAzureMonitorService
{
    [Post("{alertId}/changestate?api-version=2019-03-01&newState={azureAction}")]
    public Task<HttpResponseMessage> ChangeUserResponseAsync(string alertId, string action, [Body] AzurePayload payload, [Authorize("Bearer")] string accesstoken);
}

public class AzurePayload
{
    public AzurePayload(string comment) => Comment = comment;

    public string Comment { get; set; }
}
