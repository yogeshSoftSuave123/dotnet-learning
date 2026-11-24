namespace Day07.DI.Middleware.Services;

/// <summary>
/// Service for tracking application-wide request count
/// Registered as Singleton - single instance for application lifetime
/// </summary>
public interface IRequestCounterService
{
    int GetCount();
    void Increment();
    DateTime StartTime { get; }
}
