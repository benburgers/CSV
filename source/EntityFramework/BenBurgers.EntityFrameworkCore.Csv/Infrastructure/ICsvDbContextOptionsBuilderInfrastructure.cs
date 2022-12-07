/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore;

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure;

/// <summary>
/// Explicitly implemented by <see cref="CsvDbContextOptionsBuilder" /> to hide methods that are used by database provider extension methods but not intended to be called by application developers.
/// </summary>
public interface ICsvDbContextOptionsBuilderInfrastructure
{
    /// <summary>
    /// Gets the core options builder.
    /// </summary>
    DbContextOptionsBuilder OptionsBuilder { get; }
}
