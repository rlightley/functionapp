namespace Alerting.Functions.Opsgenie.Infrastructure.Services;

public interface IIncidentManagementService
{
    [Post("/")]
    public Task<HttpResponseMessage> SendAlertAsync([Body]OpsgenieAlert alert, [Header("Authorization")] string authorization);

}