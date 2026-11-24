using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Day07.DI.Middleware.Services;

namespace Day07.Tests;

public class ServiceLifetimeTests
{
    [Fact]
    public void Transient_Services_HaveDifferentInstances()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<IGuidGeneratorService, GuidGeneratorService>();
        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service1 = serviceProvider.GetRequiredService<IGuidGeneratorService>();
        var service2 = serviceProvider.GetRequiredService<IGuidGeneratorService>();

        // Assert
        Assert.NotEqual(service1.ServiceInstanceId, service2.ServiceInstanceId);
    }

    [Fact]
    public void Singleton_Services_HaveSameInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IRequestCounterService, RequestCounterService>();
        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service1 = serviceProvider.GetRequiredService<IRequestCounterService>();
        var service2 = serviceProvider.GetRequiredService<IRequestCounterService>();

        // Assert
        Assert.Same(service1, service2);
    }

    [Fact]
    public void Scoped_Services_HaveSameInstanceWithinScope()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IEmployeeRepository, InMemoryEmployeeRepository>();
        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        using (var scope1 = serviceProvider.CreateScope())
        {
            var service1a = scope1.ServiceProvider.GetRequiredService<IEmployeeRepository>();
            var service1b = scope1.ServiceProvider.GetRequiredService<IEmployeeRepository>();
            Assert.Same(service1a, service1b); // Same within scope
        }

        using (var scope2 = serviceProvider.CreateScope())
        {
            var service2 = scope2.ServiceProvider.GetRequiredService<IEmployeeRepository>();

            using (var scope1 = serviceProvider.CreateScope())
            {
                var service1 = scope1.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                Assert.NotSame(service1, service2); // Different across scopes
            }
        }
    }

    [Fact]
    public void RequestCounter_Singleton_MaintainsState()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IRequestCounterService, RequestCounterService>();
        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();
        var counter = serviceProvider.GetRequiredService<IRequestCounterService>();

        // Act
        counter.Increment();
        counter.Increment();
        counter.Increment();

        var count1 = counter.GetCount();

        // Get the service again
        var counter2 = serviceProvider.GetRequiredService<IRequestCounterService>();
        var count2 = counter2.GetCount();

        // Assert
        Assert.Equal(3, count1);
        Assert.Equal(3, count2); // Same state
        Assert.Same(counter, counter2);
    }
}
