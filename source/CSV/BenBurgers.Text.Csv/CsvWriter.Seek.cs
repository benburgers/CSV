/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

public partial class CsvWriter
{
    /// <summary>
    /// Flushes the buffer and seeks on the stream to the specified <paramref name="offset" /> from the specified <paramref name="origin" />.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="origin">The origin.</param>
    internal void Seek(long offset, SeekOrigin origin)
    {
        this.streamWriter.Flush();
        var baseStream = this.streamWriter.BaseStream;
        baseStream.Seek(offset, origin);
        this.streamWriter = new StreamWriter(baseStream);
        // TODO verify start of line
    }
}
