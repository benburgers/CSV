/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;

namespace BenBurgers.Text.Csv;

public partial class CsvWriter
{
    /// <summary>
    /// Writes a line to the CSV data.
    /// </summary>
    /// <param name="values">The CSV values to write.</param>
    /// <exception cref="CsvValuesDoNotMatchColumnsException">
    /// A <see cref="CsvValuesDoNotMatchColumnsException" /> is thrown if the number of values does not match the number of predefined or predetermined columns.
    /// </exception>
    public void WriteLine(IReadOnlyList<string> values)
    {
        if (this.Options.HasHeaderLine && this.ColumnNames is { } columnNames && columnNames.Count != values.Count)
            throw new CsvValuesDoNotMatchColumnsException(columnNames.Count, values.Count);
        this.streamWriter.WriteLine(string.Join(this.Options.Delimiter, values));
    }

    /// <summary>
    /// Writes a line to the CSV data.
    /// </summary>
    /// <param name="values">The CSV values to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    /// <exception cref="OperationCanceledException">
    /// An <see cref="OperationCanceledException" /> is thrown if <paramref name="cancellationToken" /> is triggered.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// An <see cref="ObjectDisposedException" /> is thrown if <paramref name="cancellationToken" /> has been disposed and is triggered.
    /// </exception>
    /// <exception cref="CsvValuesDoNotMatchColumnsException">
    /// A <see cref="CsvValuesDoNotMatchColumnsException" /> is thrown if the number of values does not match the number of predefined or predetermined columns.
    /// </exception>
    public async Task WriteLineAsync(
        IReadOnlyList<string> values,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (this.Options.HasHeaderLine && this.ColumnNames is { } columnNames && columnNames.Count != values.Count)
            throw new CsvValuesDoNotMatchColumnsException(columnNames.Count, values.Count);
        await this.streamWriter.WriteLineAsync(string.Join(this.Options.Delimiter, values));
    }
}
