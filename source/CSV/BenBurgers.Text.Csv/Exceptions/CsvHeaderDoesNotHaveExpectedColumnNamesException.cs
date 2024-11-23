/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Exceptions;

/// <summary>
/// An exception that is thrown if a CSV header line does not contain the expected column names.
/// </summary>
public sealed class CsvHeaderDoesNotHaveExpectedColumnNamesException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvHeaderDoesNotHaveExpectedColumnNamesException" />.
    /// </summary>
    /// <param name="expected">The expected column names.</param>
    /// <param name="actual">The actual column names.</param>
    internal CsvHeaderDoesNotHaveExpectedColumnNamesException(IReadOnlyList<string> expected, IReadOnlyList<string> actual)
        : base(ExceptionMessages.HeaderDoesNotHaveExpectedColumnNames)
    {
        this.Expected = expected;
        this.Actual = actual;
    }

    /// <summary>
    /// Gets the actual column names.
    /// </summary>
    public IReadOnlyList<string> Actual { get; }

    /// <summary>
    /// Gets the expected column names.
    /// </summary>
    public IReadOnlyList<string> Expected { get; }
}
