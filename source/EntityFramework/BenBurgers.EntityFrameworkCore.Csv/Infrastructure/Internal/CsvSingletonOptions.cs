/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;

/// <summary>
/// Singleton CSV options.
/// </summary>
internal sealed class CsvSingletonOptions : ICsvSingletonOptions
{
    /// <inheritdoc />
    public DirectoryInfo? DirectoryDefault { get; private set; }

    /// <inheritdoc />
    public void Initialize(IDbContextOptions options)
    {
        var csvOptions = options.FindExtension<CsvOptionsExtension>();
        if (csvOptions is not null)
        {
            this.DirectoryDefault = csvOptions.DirectoryDefault;
        }
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// An <see cref="InvalidOperationException" /> is thrown if the <paramref name="options" /> changed.
    /// </exception>
    public void Validate(IDbContextOptions options)
    {
        var csvOptions = options.FindExtension<CsvOptionsExtension>();

        if (csvOptions is not null
            && this.DirectoryDefault?.FullName != csvOptions.DirectoryDefault?.FullName)
        {
            throw new InvalidOperationException(
                CoreStrings.SingletonOptionChanged(
                    nameof(CsvDbContextOptionsExtensions.UseCsv),
                    nameof(DbContextOptionsBuilder.UseInternalServiceProvider)));
        }
    }
}
