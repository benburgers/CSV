/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BenBurgers.Text.Csv.Tests.Bootstrapping;

public class ConfigurationTestFixture
{
    public ConfigurationTestFixture()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        var configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();
        this.ServiceProvider =
            new ServiceCollection()
                .Configure<ConfigurationOptions>(configuration.GetSection("Test"))
                .BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; }
}
