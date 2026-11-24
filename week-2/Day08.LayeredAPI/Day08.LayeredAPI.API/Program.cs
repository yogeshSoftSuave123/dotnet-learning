using Day08.LayeredAPI.API.Middleware;
using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Application.Services;
using Day08.LayeredAPI.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register Application Services (Scoped - per request)
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Register Infrastructure Repositories (Scoped - per request)
builder.Services.AddScoped<IEmployeeRepository, InMemoryEmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, InMemoryDepartmentRepository>();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Management API - Layered Architecture",
        Description = "Day 08: Layered architecture with validation and exception handling"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline

// 1. Exception Handling (must be first to catch all exceptions)
app.UseExceptionHandling();

// 2. Swagger (Development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. HTTPS Redirection
app.UseHttpsRedirection();

// 4. Routing
app.UseRouting();

// 5. Authorization
app.UseAuthorization();

// 6. Map Controllers
app.MapControllers();

app.Run();
