/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Builders;
using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Metadata.Builders;

[Collection(nameof(TestConfigurationTestCollection))]
public class CsvModelBuilderExtensionsTests
{
    private readonly TestConfigurationFixture configurationFixture;

    public CsvModelBuilderExtensionsTests(TestConfigurationFixture configurationFixture)
    {
        this.configurationFixture = configurationFixture;
    }

    [Fact]
    public void HasCsvDataSourceDefaultTest()
    {
        // Arrange
        var testOptions =
            this
                .configurationFixture
                .ServiceProvider
                .GetRequiredService<IOptions<TestConfigurationOptions>>()
                .Value;

        var modelBuilder = new ModelBuilder(new ConventionSet());
        var filePath = Path.Combine(testOptions.DirectoryDefault, "HasCsvDataSourceDefault.csv");
        var csvDataSourceExpected = new CsvFileSource(filePath);

        // Act
        modelBuilder.HasCsvDataSourceDefault(csvDataSourceExpected);

        // Assert
        var annotation = modelBuilder.Model.GetAnnotation(CsvAnnotationNames.DataSource);
        Assert.NotNull(annotation.Value);
        var csvDataSourceActual = Assert.IsType<CsvFileSource>(annotation.Value);
        Assert.Equal(filePath, csvDataSourceActual.Path);
    }
}
