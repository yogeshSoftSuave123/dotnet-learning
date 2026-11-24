namespace Day07.DI.Middleware.Configuration;

/// <summary>
/// Application settings configuration class
/// Demonstrates configuration binding
/// </summary>
public class AppSettings
{
    public const string SectionName = "AppSettings";

    public string ApplicationName { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public FeatureFlags Features { get; set; } = new();
    public LoggingSettings Logging { get; set; } = new();
}

public class FeatureFlags
{
    public bool EnableDetailedErrors { get; set; }
    public bool EnableRequestLogging { get; set; } = true;
    public bool EnablePerformanceMonitoring { get; set; }
}

public class LoggingSettings
{
    public string MinimumLevel { get; set; } = "Information";
    public bool IncludeScopes { get; set; } = true;
}
