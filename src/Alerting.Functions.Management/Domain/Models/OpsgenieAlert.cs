namespace Alerting.Functions.Management.Domain.Models;
public class OpsgenieAlert
{
    [JsonProperty("message")]
    public string Message { get; set; } // Mandatory
    [JsonProperty("alias")]
    public string Alias { get; set; } // Optional
    [JsonProperty("descritpion")]
    public string Description { get; set; } // Optional
    [JsonProperty("responders")]
    public List<Responder> Responders { get; set; } // Optional
    [JsonProperty("visibleTo")]
    public List<IsVisibleTo> VisibleTo { get; set; } // Optional
    [JsonProperty("actions")]
    public List<string> Actions { get; set; } // Optional
    [JsonProperty("tags")]
    public List<string> Tags { get; set; } // Optional
    [JsonProperty("details")]
    public Dictionary<string, string> Details { get; set; } // Optional
    [JsonProperty("entity")]
    public string Entity { get; set; } // Optional
    [JsonProperty("source")]
    public string Source { get; set; } // Optional
    [JsonProperty("priority")]
    public string Priority { get; set; } // Optional
    [JsonProperty("user")]
    public string User { get; set; } // Optional
    [JsonProperty("note")]
    public string Note { get; set; } // Optional


    public class Responder
    {
        [JsonProperty("type")]
        public string Type { get; set; } // Mandatory (team, user, escalation, schedule)
        [JsonProperty("id")]
        public string Id { get; set; } // Either id or name should be provided
        [JsonProperty("name")]
        public string Name { get; set; } // Either id or name should be provided
    }

    public class IsVisibleTo
    {
        [JsonProperty("type")]
        public string Type { get; set; } // Mandatory (team, user)
        [JsonProperty("id")]
        public string Id { get; set; } // Either id or username should be provided (for teams, users)
        [JsonProperty("username")]
        public string Username { get; set; } // Either id or username should be provided (for teams, users)
    }
}

public static class AzureAlertExtensions
{
    public static OpsgenieAlert ToOpsgenieAlert(this AzureAlert alert)
    {
        return new OpsgenieAlert
        {
            Message = $"{alert.Data.Essentials.AlertRule} {DateTime.UtcNow}",
            Description = alert.Data.Essentials.Description,
            Alias = alert.Data.Essentials.AlertId,
            Details = alert.Data.Essentials.CustomProperties,
            Priority = SetPriority(alert.Data.Essentials.Severity)
        };
    }

    private static string SetPriority(string severity)
    {
        switch (severity)
        {
            case "Sev1": return "P1";
            case "Sev2": return "P2";
            case "Sev3": return "P3";
            default: return "P3";
        }
    }
}