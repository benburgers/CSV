/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Extensions;
using BenBurgers.EntityFrameworkCore.Csv.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Extensions;

[Collection(nameof(TestConfigurationTestCollection))]
public class CsvServiceCollectionExtensionsTests
{
    private readonly TestConfigurationFixture configurationFixture;

    public CsvServiceCollectionExtensionsTests(TestConfigurationFixture configurationFixture)
    {
        this.configurationFixture = configurationFixture;
    }

    [Fact]
    public void AddCsvTest()
    {
        // Arrange
        var directoryDefault =
            this
                .configurationFixture
                .ServiceProvider
                .GetRequiredService<IOptions<TestConfigurationOptions>>()
                .Value
                .DirectoryDefault;
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddCsv<CsvDbContext>(new DirectoryInfo(directoryDefault));

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var csvDbContext = serviceProvider.GetRequiredService<CsvDbContext>();
        Assert.NotNull(csvDbContext);
    }
}
