using Day07.DI.Middleware.Services;
using Day07.DI.Middleware.Middleware;
using Day07.DI.Middleware.Configuration;

var builder = WebApplication.CreateBuilder(args);

// ===== Configuration =====
// Bind configuration to strongly-typed options
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(AppSettings.SectionName));

// ===== Dependency Injection Registration =====

// Add controllers
builder.Services.AddControllers();

// TRANSIENT: New instance every time requested
// Good for: Lightweight, stateless services
builder.Services.AddTransient<IGuidGeneratorService, GuidGeneratorService>();

// SCOPED: New instance per request (HTTP request scope)
// Good for: Request-specific operations, repositories, services
builder.Services.AddScoped<IEmployeeRepository, InMemoryEmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// SINGLETON: Single instance for application lifetime
// Good for: Shared state, caching, configuration
builder.Services.AddSingleton<IRequestCounterService, RequestCounterService>();

// ===== Logging Configuration =====
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ===== Swagger/OpenAPI =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Middleware Pipeline Configuration =====
// Order matters! Middleware executes in the order registered

// 1. Swagger (Development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. HTTPS Redirection
app.UseHttpsRedirection();

// 3. Custom Request Counter (must be early to count all requests)
app.UseRequestCounter();

// 4. Custom Request Logging (logs request/response metadata)
app.UseRequestLogging();

// 5. Routing (matches requests to endpoints)
app.UseRouting();

// 6. Authentication (would go here if implemented)
// app.UseAuthentication();

// 7. Authorization (would go here if implemented)
app.UseAuthorization();

// 8. Endpoints (controllers)
app.MapControllers();

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application starting in {Environment} environment", app.Environment.EnvironmentName);
logger.LogInformation("DI Container configured with:");
logger.LogInformation("  - Transient: IGuidGeneratorService");
logger.LogInformation("  - Scoped: IEmployeeRepository, IEmployeeService");
logger.LogInformation("  - Singleton: IRequestCounterService");

app.Run();
