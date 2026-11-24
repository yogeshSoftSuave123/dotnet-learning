namespace Day08.LayeredAPI.Application.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found
/// Used for consistent error handling across the application
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : base($"{name} with key '{key}' was not found")
    {
    }
}
