/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Infrastructure;
using BenBurgers.EntityFrameworkCore.Csv.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Storage;

[Collection(nameof(TestConfigurationTestCollection))]
public class DatabaseTests
{
    private readonly TestConfigurationFixture configurationFixture;

    public DatabaseTests(TestConfigurationFixture configurationFixture)
    {
        this.configurationFixture = configurationFixture;
    }

    [Fact]
    public async Task EnsureCreatedTestAsync()
    {
        // Arrange

        /* Test Options */
        var testOptions =
            this
                .configurationFixture
                .ServiceProvider
                .GetRequiredService<IOptions<TestConfigurationOptions>>()
                .Value;
        var directoryDefault = new DirectoryInfo(testOptions.DirectoryDefault);

        /* DbContext */
        var dbContextOptions =
            new DbContextOptionsBuilder<CsvDbContext>()
                .UseCsv(directoryDefault)
                .Options;
        var dbContext = new CsvDbContext(dbContextOptions);

        // Act
        await dbContext.Database.EnsureCreatedAsync();

        // Assert
        var files = directoryDefault.GetFiles();
        var file = files.FirstOrDefault(f => f.Name == $"{nameof(MockRecord)}Test.csv");
        Assert.NotNull(file);

    }
}
