using Day08.LayeredAPI.API.Middleware;
using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Application.Services;
using Day08.LayeredAPI.Infrastructure.Repositories;
using Day08.LayeredAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register DbContext (Day 09: EF Core)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Application Services (Scoped - per request)
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Register EF Core Repositories (Day 09)
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, EfDepartmentRepository>();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Management API - with EF Core",
        Description = "Days 08-09: Layered architecture with EF Core database"
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
