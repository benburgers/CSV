/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure;

/// <summary>
/// Allows CSV specific configuration to be performed on <see cref="DbContextOptions" />.
/// </summary>
public sealed partial class CsvDbContextOptionsBuilder : ICsvDbContextOptionsBuilderInfrastructure
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvDbContextOptionsBuilder" />.
    /// </summary>
    /// <param name="optionsBuilder">The options builder.</param>
    public CsvDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
    {
        this.OptionsBuilder = optionsBuilder;
    }

    /// <summary>
    /// Gets the options builder.
    /// </summary>
    public DbContextOptionsBuilder OptionsBuilder { get; }

    /// <summary>
    /// Configures the context to use the provided <paramref name="directoryDefault" />.
    /// </summary>
    /// <param name="directoryDefault">The default directory that contains CSV files.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public CsvDbContextOptionsBuilder DirectoryDefault(string directoryDefault)
        => this.DirectoryDefault(new DirectoryInfo(directoryDefault));

    /// <summary>
    /// Configures the context to use the provided <paramref name="directoryDefault" />.
    /// </summary>
    /// <param name="directoryDefault">The default directory that contains CSV files.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public CsvDbContextOptionsBuilder DirectoryDefault(DirectoryInfo directoryDefault)
        => this.WithOption(e => e.WithDirectoryDefault(directoryDefault));

    /// <summary>
    /// Sets an option by cloning the extension used to store the settings.
    /// This ensures the builder does not modify options that are already in use elsewhere.
    /// </summary>
    /// <param name="setAction">An action to set the option.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    private CsvDbContextOptionsBuilder WithOption(Func<CsvOptionsExtension, CsvOptionsExtension> setAction)
    {
        ((IDbContextOptionsBuilderInfrastructure)this.OptionsBuilder)
            .AddOrUpdateExtension(setAction(this.OptionsBuilder.Options.FindExtension<CsvOptionsExtension>()
                ?? new CsvOptionsExtension()));
        return this;
    }
}
