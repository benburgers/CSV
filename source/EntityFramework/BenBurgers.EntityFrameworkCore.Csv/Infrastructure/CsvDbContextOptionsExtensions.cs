/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure;

/// <summary>
/// Extension methods for <see cref="DbContextOptionsBuilder{TContext}" />.
/// </summary>
public static class CsvDbContextOptionsExtensions
{
    /// <summary>
    /// Configures the context to read and write CSV data.
    /// </summary>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="directoryDefault">The default directory that contains CSV files.</param>
    /// <param name="csvOptionsAction">An optional action to allow additional CSV-specific configuration.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder UseCsv(
        this DbContextOptionsBuilder optionsBuilder,
        DirectoryInfo directoryDefault,
        Action<CsvDbContextOptionsBuilder>? csvOptionsAction = null)
    {
        var extension = optionsBuilder.Options.FindExtension<CsvOptionsExtension>()
            ?? new CsvOptionsExtension();

        extension = extension.WithDirectoryDefault(directoryDefault);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        csvOptionsAction?.Invoke(new CsvDbContextOptionsBuilder(optionsBuilder));

        return optionsBuilder;
    }

    /// <summary>
    /// Configures the context to read and write CSV data.
    /// </summary>
    /// <typeparam name="TContext">The type of context to be configured.</typeparam>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="directoryDefault">The default directory that contains CSV files.</param>
    /// <param name="csvOptionsAction">An optional action to allow additional CSV-specific configuration.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder<TContext> UseCsv<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        DirectoryInfo directoryDefault,
        Action<CsvDbContextOptionsBuilder>? csvOptionsAction = null)
        where TContext : DbContext
    {
        var extension = optionsBuilder.Options.FindExtension<CsvOptionsExtension>()
            ?? new CsvOptionsExtension();

        extension = extension.WithDirectoryDefault(directoryDefault);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        csvOptionsAction?.Invoke(new CsvDbContextOptionsBuilder(optionsBuilder));

        return optionsBuilder;
    }
}
