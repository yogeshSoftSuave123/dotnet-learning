namespace Day07.DI.Middleware.Services;

/// <summary>
/// Service for generating GUIDs
/// Registered as Transient - new instance every time it's requested
/// </summary>
public interface IGuidGeneratorService
{
    Guid GenerateGuid();
    Guid ServiceInstanceId { get; }
}
