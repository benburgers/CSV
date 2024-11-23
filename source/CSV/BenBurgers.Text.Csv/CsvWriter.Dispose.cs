/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

public partial class CsvWriter : IDisposable, IAsyncDisposable
{
    private bool disposedValue = false;

    private void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.streamWriter.Dispose();
            }
            this.disposedValue = true;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
        await Task.CompletedTask;
    }
}
