/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;

/// <summary>
/// Options extension for CSV data.
/// </summary>
internal partial class CsvOptionsExtension : ICloneable, IDbContextOptionsExtension
{
    private ExtensionInfo? info;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvOptionsExtension" />.
    /// </summary>
    public CsvOptionsExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvOptionsExtension" />.
    /// </summary>
    /// <param name="copyFrom">The source to copy from.</param>
    public CsvOptionsExtension(CsvOptionsExtension copyFrom)
    {
        this.DirectoryDefault = copyFrom.DirectoryDefault;
    }

    /// <inheritdoc />
    public DbContextOptionsExtensionInfo Info => this.info ??= new ExtensionInfo(this);

    /// <summary>
    /// The default directory that contains CSV files.
    /// </summary>
    public DirectoryInfo? DirectoryDefault { get; private set; }

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
        => services.AddEntityFrameworkCsv();

    /// <inheritdoc />
    public void Validate(IDbContextOptions options)
    {
    }

    /// <summary>
    /// Creates a new instance with all options the same as for this instance, but with the given option changed.
    /// </summary>
    /// <param name="defaultDirectory">The default directory that contains CSV files.</param>
    /// <returns>A new instance with the option changed.</returns>
    public CsvOptionsExtension WithDirectoryDefault(DirectoryInfo? defaultDirectory)
    {
        var clone = this.Clone();
        clone.DirectoryDefault = defaultDirectory;
        return clone;
    }
}