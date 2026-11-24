using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Day07.DI.Middleware.Configuration;
using Day07.DI.Middleware.Services;

namespace Day07.DI.Middleware.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController : ControllerBase
{
    private readonly IRequestCounterService _counterService;
    private readonly IOptions<AppSettings> _appSettings;
    private readonly ILogger<DiagnosticsController> _logger;

    public DiagnosticsController(
        IRequestCounterService counterService,
        IOptions<AppSettings> appSettings,
        ILogger<DiagnosticsController> logger)
    {
        _counterService = counterService;
        _appSettings = appSettings;
        _logger = logger;
    }

    /// <summary>
    /// Get application diagnostics including request count and uptime
    /// Demonstrates singleton service access
    /// </summary>
    [HttpGet("info")]
    public ActionResult<object> GetInfo()
    {
        var uptime = DateTime.UtcNow - _counterService.StartTime;

        var info = new
        {
            ApplicationName = _appSettings.Value.ApplicationName,
            Environment = _appSettings.Value.Environment,
            TotalRequests = _counterService.GetCount(),
            StartTime = _counterService.StartTime,
            Uptime = new
            {
                Days = uptime.Days,
                Hours = uptime.Hours,
                Minutes = uptime.Minutes,
                Seconds = uptime.Seconds,
                TotalSeconds = (int)uptime.TotalSeconds
            },
            Features = _appSettings.Value.Features,
            ServerTime = DateTime.UtcNow
        };

        _logger.LogInformation("Diagnostics info requested: {TotalRequests} requests, {UptimeSeconds}s uptime",
            info.TotalRequests, info.Uptime.TotalSeconds);

        return Ok(info);
    }

    /// <summary>
    /// Get current configuration (non-sensitive)
    /// Demonstrates IOptions pattern
    /// </summary>
    [HttpGet("config")]
    public ActionResult<object> GetConfig()
    {
        return Ok(new
        {
            _appSettings.Value.ApplicationName,
            _appSettings.Value.Environment,
            _appSettings.Value.Features
        });
    }
}
