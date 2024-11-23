/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Attributes;

/// <summary>
/// An attribute that specifies information for a CSV column on either a constructor parameter or a property.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
public sealed class CsvColumnAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvColumnAttribute" />.
    /// </summary>
    /// <param name="name">The CSV column name.</param>
    public CsvColumnAttribute(string name)
    {
        this.Name = name;
    }

    /// <summary>
    /// Gets the CSV column name.
    /// </summary>
    public string Name { get; }
}
