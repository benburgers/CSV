/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

public partial class CsvWriter
{
    /// <summary>
    /// Flushes the data in the buffer to the stream.
    /// </summary>
    public void Flush()
    {
        this.streamWriter.Flush();
    }

    /// <summary>
    /// Flushes the buffer to the data stream.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    /// <exception cref="OperationCanceledException">
    /// An <see cref="OperationCanceledException" /> is thrown if <paramref name="cancellationToken" /> is triggered.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// An <see cref="ObjectDisposedException" /> is thrown if <paramref name="cancellationToken" /> has been disposed and is triggered.
    /// </exception>
    public async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await this.streamWriter.FlushAsync();
    }
}
