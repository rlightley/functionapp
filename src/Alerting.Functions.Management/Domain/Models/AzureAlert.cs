namespace Alerting.Functions.Management.Domain.Models;
public class AzureAlert
{

    public string SchemaId { get; set; }
    public AlertData Data { get; set; }

    public class AlertData
    {
        public Essentials Essentials { get; set; }
        public AlertContext AlertContext { get; set; }
    }

    public class Essentials
    {
        public string AlertId { get; set; }
        public string AlertRule { get; set; }
        public string Severity { get; set; }
        public string SignalType { get; set; }
        public string MonitorCondition { get; set; }
        public string MonitoringService { get; set; }
        public List<string> AlertTargetIDs { get; set; }
        public List<string> ConfigurationItems { get; set; }
        public string OriginAlertId { get; set; }
        public DateTime FiredDateTime { get; set; }
        public DateTime ResolvedDateTime { get; set; }
        public string Description { get; set; }
        public string EssentialsVersion { get; set; }
        public string AlertContextVersion { get; set; }
        public Dictionary<string, string> CustomProperties { get; set; }
    }

    public class AlertContext
    {
        public object Properties { get; set; }
        public string ConditionType { get; set; }
        public Condition Condition { get; set; }
    }

    public class Condition
    {
        public string WindowSize { get; set; }
        public List<Criterion> AllOf { get; set; }
    }

    public class Criterion
    {
        public string MetricName { get; set; }
        public string MetricNamespace { get; set; }
        public string Operator { get; set; }
        public string Threshold { get; set; }
        public string TimeAggregation { get; set; }
        public List<Dimension> Dimensions { get; set; }
        public double MetricValue { get; set; }
    }

    public class Dimension
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
