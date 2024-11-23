/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Bootstrapping;

public sealed class TestConfigurationFixture
{
    public TestConfigurationFixture()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        var configuration =
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", true)
                .Build();
        this.ServiceProvider =
            new ServiceCollection()
                .Configure<TestConfigurationOptions>(configuration.GetSection("Test"))
                .BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; }
}
