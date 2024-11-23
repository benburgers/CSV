/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata;
using BenBurgers.Text.Csv;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update;
using System.Text;

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <summary>
/// An adapter for a CSV data source.
/// </summary>
internal sealed class CsvFileSourceAdapter : ICsvDataSourceAdapter
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvFileSourceAdapter" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    /// <param name="fileSource">The file source.</param>
    internal CsvFileSourceAdapter(
        CsvDataSourceAdapterDependencies dependencies,
        CsvFileSource fileSource)
    {
        this.Dependencies = dependencies;
        this.FileSource = fileSource;
    }

    /// <summary>
    /// Gets the dependencies.
    /// </summary>
    public CsvDataSourceAdapterDependencies Dependencies { get; }

    /// <summary>
    /// Gets the file source.
    /// </summary>
    public CsvFileSource FileSource { get; }

    private static IEnumerable<string> GetValues(IUpdateEntry entry)
    {
        var properties =
            entry
                .EntityType
                .GetProperties()
                .OrderBy(p => p.GetCsvColumn().ColumnIndex)
                .ToArray();
        foreach (var property in properties)
        {
            yield return property.GetValueConverter() is { } valueConverter
                ? valueConverter.ConvertToProvider(entry.GetCurrentValue(property))!.ToString() ?? string.Empty
                : entry.GetCurrentValue(property)!.ToString() ?? string.Empty;
        }
    }

    private static StringBuilder WriteValues(StringBuilder stringBuilder, char delimiter, params string[] values)
        => stringBuilder.AppendLine(string.Join(delimiter, values));

    /// <inheritdoc />
    public async Task<bool> CreateIfNotExistsAsync(
        IReadOnlyEntityType entityType,
        CancellationToken cancellationToken = default)
    {
        var path = this.FileSource.Path;
        if (!Path.IsPathRooted(path))
        {
            var directoryDefault = this.Dependencies.Options.DirectoryDefault?.FullName ?? Directory.GetCurrentDirectory();
            path = Path.Combine(directoryDefault, path);
        }
        if (!File.Exists(path))
        {
            using var fileStream = File.Create(path);
            var csvOptions =
                new CsvOptions(
                    ColumnNames: entityType.GetCsvColumns().Select(c => c.ColumnName!).ToArray(),
                    Delimiter: this.FileSource.Delimiter,
                    HasHeaderLine: this.FileSource.HasHeaderLine);
            await using var csvWriter = new CsvWriter(fileStream, csvOptions);
            return true;
        }
        return false;
        // TODO if exists and has column names row, validate column names
    }

    /// <inheritdoc />
    public async Task WriteNewAsync(IUpdateEntry entry, CancellationToken cancellationToken = default)
    {
        var path = this.FileSource.Path;
        if (!Path.IsPathRooted(path))
            path = Path.Combine(this.Dependencies.Options.DirectoryDefault?.FullName ?? Directory.GetCurrentDirectory(), path);
        var values = GetValues(entry).ToArray();

        using var streamWriter = File.AppendText(path);
        var stringBuilder = new StringBuilder();
        stringBuilder = WriteValues(stringBuilder, this.FileSource.Delimiter, values);
        await streamWriter.WriteAsync(stringBuilder.ToString());
        streamWriter.Close();
    }

    CsvDataSource ICsvDataSourceAdapter.DataSource => this.FileSource;
}
