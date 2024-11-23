/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

public partial class CsvStream
{
    private void PositionIncrement(IReadOnlyList<string> values)
    {
        // The stream reader and stream writer have their own buffers, so we need to track the position ourselves.
        // We can determine the new position by adding the bytes of the encoded values as well as the delimiters between them and the new line characters.
        this.Position +=
            values
                .Select(v => this.reader.Encoding.GetBytes(v).Length)
                .Sum()
                + values.Count - 1 // delimiters
                + this.reader.Encoding.GetBytes(this.writer.NewLine).Length; // line break and/or carriage return
    }

    /// <summary>
    /// Reads a line from the stream.
    /// </summary>
    /// <returns>A list of raw values.</returns>
    public IReadOnlyList<string>? ReadLine()
    {
        var position = this.Position;
        var values = this.reader.ReadLine();
        if (values is not null)
        {
            this.Line++;
            this.LineIndex[this.Line] = position;
            this.PositionIncrement(values);
        }
        return values;
    }

    /// <summary>
    /// Reads a line from the stream.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of raw values.</returns>
    public async Task<IReadOnlyList<string>?> ReadLineAsync(CancellationToken cancellationToken = default)
    {
        var position = this.Position;
        var values = await this.reader.ReadLineAsync(cancellationToken);
        if (values is not null)
        {
            this.Line++;
            this.LineIndex[this.Line] = position;
            this.PositionIncrement(values);
        }
        return values;
    }
}
