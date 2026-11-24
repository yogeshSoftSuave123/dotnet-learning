namespace Day07.DI.Middleware.Services;

/// <summary>
/// Singleton service that tracks total requests across the application
/// Demonstrates thread-safe singleton pattern
/// </summary>
public class RequestCounterService : IRequestCounterService
{
    private int _count = 0;
    private readonly object _lock = new();
    private readonly Guid _instanceId = Guid.NewGuid();

    public DateTime StartTime { get; } = DateTime.UtcNow;

    public RequestCounterService(ILogger<RequestCounterService> logger)
    {
        logger.LogInformation("RequestCounterService created as SINGLETON with instance ID: {InstanceId}", _instanceId);
    }

    public int GetCount()
    {
        lock (_lock)
        {
            return _count;
        }
    }

    public void Increment()
    {
        lock (_lock)
        {
            _count++;
        }
    }
}
