/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;

namespace BenBurgers.Text.Csv.Mapping.Exceptions;

/// <summary>
/// An exception that is 
/// </summary>
public sealed class CsvHeaderTypeMappingNoSuitableConstructorFoundException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvHeaderTypeMappingNoSuitableConstructorFoundException" />.
    /// </summary>
    internal CsvHeaderTypeMappingNoSuitableConstructorFoundException()
        : base(ExceptionMessages.HeaderTypeMappingNoSuitableConstructorFound)
    {
    }
}
