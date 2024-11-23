/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

public partial class CsvWriter
{
    /// <summary>
    /// Ensures that the end of the stream contains a new line (line feed (LF) and/or carriage return (CR)).
    /// </summary>
    /// <returns>
    /// The position at the end of the stream.
    /// </returns>
    internal long EnsureEndOfStreamNewLine()
    {
        // Flush any data that hasn't been persisted to the stream yet.
        this.streamWriter.Flush();

        // Seek to the end of the stream but before new line characters should be present.
        var newLineLength = this.streamWriter.NewLine.Length;
        var stream = this.streamWriter.BaseStream;
        stream.Seek(-newLineLength, SeekOrigin.End);

        // Read only the characters that should be new line characters.
        var buffer = new byte[newLineLength];
        stream.Read(buffer, 0, newLineLength);

        // If the characters are not new line characters, then add new line characters and flush the buffer immediately.
        var tail = this.streamWriter.Encoding.GetString(buffer);
        if (tail != this.streamWriter.NewLine)
        {
            this.streamWriter.WriteLine();
            this.streamWriter.Flush();
        }
        
        // Return the (new) end-of-stream position.
        return stream.Position;
    }
}
