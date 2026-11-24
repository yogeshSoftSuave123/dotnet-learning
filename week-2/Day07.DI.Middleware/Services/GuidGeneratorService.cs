namespace Day07.DI.Middleware.Services;

/// <summary>
/// Transient service - lightweight, stateless operations
/// New instance created each time it's requested
/// </summary>
public class GuidGeneratorService : IGuidGeneratorService
{
    public Guid ServiceInstanceId { get; } = Guid.NewGuid();

    public GuidGeneratorService(ILogger<GuidGeneratorService> logger)
    {
        logger.LogDebug("GuidGeneratorService created as TRANSIENT with instance ID: {InstanceId}", ServiceInstanceId);
    }

    public Guid GenerateGuid()
    {
        return Guid.NewGuid();
    }
}
