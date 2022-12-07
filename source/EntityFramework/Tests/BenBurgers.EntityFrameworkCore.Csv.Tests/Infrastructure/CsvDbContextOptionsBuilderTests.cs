/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Extensions;
using BenBurgers.EntityFrameworkCore.Csv.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Infrastructure;

[Collection(nameof(TestConfigurationTestCollection))]
public class CsvDbContextOptionsBuilderTests
{
    private readonly TestConfigurationFixture configurationFixture;

    public CsvDbContextOptionsBuilderTests(TestConfigurationFixture configurationFixture)
    {
        this.configurationFixture = configurationFixture;
    }

    [Fact(DisplayName = "CSV Database Context Options Builder :: Directory Default (string)")]
    public void DirectoryDefaultStringTest()
    {
        // Arrange
        var testOptions = this.configurationFixture.ServiceProvider.GetRequiredService<IOptions<TestConfigurationOptions>>().Value;
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddCsv<CsvDbContext>(new DirectoryInfo(testOptions.DirectoryDefault));

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var csvDbContext = serviceProvider.GetRequiredService<CsvDbContext>();
        Assert.NotNull(csvDbContext);
    }

    [Fact(DisplayName = "CSV Database Context Options Builder :: Directory Default (Directory Info)")]
    public void DirectoryDefaultInfoTest()
    {
        // Arrange
        var testOptions = this.configurationFixture.ServiceProvider.GetRequiredService<IOptions<TestConfigurationOptions>>().Value;
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddCsv<CsvDbContext>(new DirectoryInfo(testOptions.DirectoryDefault));

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var csvDbContext = serviceProvider.GetRequiredService<CsvDbContext>();
        Assert.NotNull(csvDbContext);
    }
}
