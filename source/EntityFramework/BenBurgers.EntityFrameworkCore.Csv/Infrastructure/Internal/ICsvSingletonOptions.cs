/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;

/// <summary>
/// Singleton CSV options.
/// </summary>
internal interface ICsvSingletonOptions : ISingletonOptions
{
    /// <summary>
    /// Gets the default directory that contains CSV files.
    /// </summary>
    DirectoryInfo? DirectoryDefault { get; }
}
