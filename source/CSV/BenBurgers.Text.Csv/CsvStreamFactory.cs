/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

/// <inheritdoc />
public sealed class CsvStreamFactory : ICsvStreamFactory
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvStreamFactory" />.
    /// </summary>
    /// <param name="options">The CSV configuration options.</param>
    public CsvStreamFactory(CsvOptions options)
    {
        this.Options = options;
    }

    /// <summary>
    /// Gets the CSV options.
    /// </summary>
    public CsvOptions Options { get; }

    /// <inheritdoc />
    public CsvStream From(FileInfo file)
    {
        var stream = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        return new(stream, this.Options);
    }

    /// <inheritdoc />
    public CsvStream From(
        HttpClient httpClient,
        HttpRequestMessage downloadMessage,
        HttpRequestMessage uploadMessage,
        TimeSpan updateInterval)
    {
        // Download the CSV data.
        var responseMessage = httpClient.Send(downloadMessage);
        responseMessage.EnsureSuccessStatusCode();

        // Copy the CSV data to a local memory buffer.
        var memoryStream = new MemoryStream();
        responseMessage.Content.ReadAsStream().CopyTo(memoryStream);
        memoryStream.Seek(0L, SeekOrigin.Begin);

        // Create the CSV stream.
        var csvStream = new CsvStream(memoryStream, this.Options);
        
        // Periodically reupload the CSV data.
        var update = new Thread(() =>
        {
            if (!csvStream.Open)
                return;
            Thread.Sleep(updateInterval);
            var bufferedBytes = memoryStream.ToArray();
            uploadMessage.Content = new ByteArrayContent(bufferedBytes);
            httpClient.Send(uploadMessage);
        });
        update.Start();

        // Return the CSV stream.
        return csvStream;
    }

    /// <inheritdoc />
    public CsvStream From(Stream stream)
        => new(stream, this.Options);

    /// <inheritdoc />
    public CsvStream FromFile(string fileName)
        => this.From(new FileInfo(fileName));
}
