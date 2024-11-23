/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Infrastructure;
using BenBurgers.EntityFrameworkCore.Csv.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Query;

[Collection(nameof(TestConfigurationTestCollection))]
public class QueryTests
{
    private readonly TestConfigurationFixture configurationFixture;

    public QueryTests(TestConfigurationFixture configurationFixture)
    {
        this.configurationFixture = configurationFixture;
    }

    [Fact]
    public void WhereTest()
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
        var items =
            dbContext
                .Set<MockRecord>()
                .Where(r => r.MockString == "Test 2")
                .ToArray();

        // Assert
        Assert.NotEmpty(items);
    }
}
