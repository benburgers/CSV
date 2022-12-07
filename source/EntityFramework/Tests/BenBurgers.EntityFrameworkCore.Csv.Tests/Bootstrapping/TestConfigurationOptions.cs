/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Bootstrapping;

public sealed class TestConfigurationOptions
{
    /// <summary>
    /// Gets or sets the default directory that contains test CSV files.
    /// </summary>
    public string DirectoryDefault { get; init; } = default!;
}
