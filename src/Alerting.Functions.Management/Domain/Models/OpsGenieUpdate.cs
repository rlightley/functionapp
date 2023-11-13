namespace Alerting.Functions.Management.Domain.Models;
public class OpsGenieUpdate
{
    public string Action { get; set; }
    public AlertData Alert { get; set; }

    public class AlertData
    {
        public string Alias { get; set; }
    }
}
