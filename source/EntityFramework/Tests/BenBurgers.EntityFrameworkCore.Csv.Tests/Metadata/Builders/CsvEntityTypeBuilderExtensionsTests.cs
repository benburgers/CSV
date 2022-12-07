/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Builders;
using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Storage;
using BenBurgers.EntityFrameworkCore.Csv.Tests.Mocks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Metadata.Builders;

[Collection(nameof(TestConfigurationTestCollection))]
public class CsvEntityTypeBuilderExtensionsTests
{
    private readonly TestConfigurationFixture configurationFixture;

    public CsvEntityTypeBuilderExtensionsTests(TestConfigurationFixture configurationFixture)
    {
        this.configurationFixture = configurationFixture;
    }

    [Fact]
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "Unit testing")]
    public void ToCsvDataSourceTest()
    {
        // Arrange
        var testOptions =
            this
                .configurationFixture
                .ServiceProvider
                .GetRequiredService<IOptions<TestConfigurationOptions>>()
                .Value;

        var entityType = new EntityType(nameof(MockRecord), new Model(), false, ConfigurationSource.Explicit);
        var entityTypeBuilder = new EntityTypeBuilder<MockRecord>(entityType);

        var filePath = Path.Combine(testOptions.DirectoryDefault, "CsvDataSourceTest.csv");
        var csvDataSourceExpected = new CsvFileSource(filePath);

        // Act
        entityTypeBuilder.ToCsvDataSource(csvDataSourceExpected);

        // Assert
        var annotation = entityTypeBuilder.Metadata.GetAnnotation(CsvAnnotationNames.DataSource);
        Assert.NotNull(annotation.Value);
        var csvDataSourceActual = Assert.IsType<CsvFileSource>(annotation.Value);
        Assert.Equal(filePath, csvDataSourceActual.Path);
    }
}
