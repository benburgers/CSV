/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Tests.Bootstrapping;

public class ConfigurationOptions
{
    public string TestDirectoryName { get; init; } = default!;
    public int ProxyPort { get; init; } = 8080;
}
