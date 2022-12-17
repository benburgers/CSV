/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

/// <summary>
/// A factory for CSV streams.
/// </summary>
internal interface ICsvStreamFactory
{
    /// <summary>
    /// Creates a CSV stream from a file.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <returns>The CSV stream.</returns>
    CsvStream From(FileInfo file);

    /// <summary>
    /// Creates a CSV stream from an HTTP Client and an HTTP Request Message.
    /// </summary>
    /// <param name="httpClient">The HTTP Client.</param>
    /// <param name="downloadMessage">The HTTP Request Message for downloading the CSV data.</param>
    /// <param name="uploadMessage">The HTTP Request Message for uploading the CSV data.</param>
    /// <param name="updateInterval">The interval after which the HTTP Client sends the contents of the memory buffer with possibly updated CSV data to the CSV data source.</param>
    /// <returns>The CSV stream.</returns>
    CsvStream From(
        HttpClient httpClient,
        HttpRequestMessage downloadMessage,
        HttpRequestMessage uploadMessage,
        TimeSpan updateInterval);

    /// <summary>
    /// Creates a CSV stream from a data stream.
    /// </summary>
    /// <param name="stream">The stream from which to create a CSV stream.</param>
    /// <returns>The CSV stream.</returns>
    CsvStream From(Stream stream);

    /// <summary>
    /// Creates a CSV stream from a file.
    /// </summary>
    /// <param name="fileName">The file name.</param>
    /// <returns>The CSV stream.</returns>
    CsvStream FromFile(string fileName);
}
