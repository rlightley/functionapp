namespace Alerting.Functions.Management.Infrastructure.Mediator;

public class MediatorLoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : class
{
    private readonly ILogger<MediatorLoggingBehaviour<TRequest, TResponse>> _logger;

    public MediatorLoggingBehaviour(ILogger<MediatorLoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {request}", typeof(TRequest).FullName);
        Type type = request.GetType();

        IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

        foreach (PropertyInfo prop in props)
        {
            object propValue = prop.GetValue(request, null);
            _logger.LogInformation("{Property} : {@Value}", prop.Name, propValue);
        }

        TResponse response;
        try
        {
            response = await next();
            _logger.LogInformation($"Successfully handled {request}", typeof(TRequest).FullName);
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Error Handling {request}: {error}. Stack trace: {stacktrace}", typeof(TRequest).FullName, e.Message, e.StackTrace);
            return new Microsoft.AspNetCore.Mvc.ProblemDetails() { Status = 500, Detail = e.Message, Type = e.GetType().Name } as TResponse;
        }

    }
}
