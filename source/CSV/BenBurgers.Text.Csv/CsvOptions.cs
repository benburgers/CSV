/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Mapping;

namespace BenBurgers.Text.Csv;

/// <summary>
/// Configuration options for CSV data.
/// </summary>
/// <param name="CodePage">The code page of the encoding, <see langword="null" /> means the encoding is detected automatically. Defaults to <see langword="null" />.</param>
/// <param name="Delimiter">The delimiter of columns. Defaults to ','.</param>
/// <param name="HasHeaderLine">Indicates whether the CSV data has a header line with column names. Defaults to 'false'.</param>
/// <param name="ColumnNames">A set of predefined column names.</param>
public record CsvOptions(
    int? CodePage = null,
    char Delimiter = ',',
    bool HasHeaderLine = false,
    IReadOnlySet<string>? ColumnNames = null)
{
}

/// <summary>
/// Configuration options for CSV data.
/// </summary>
/// <typeparam name="T">The type of the CSV records.</typeparam>
/// <param name="CodePage">The code page of the encoding, <see langword="null" /> means the encoding is detected automatically. Defaults to <see langword="null" />.</param>
/// <param name="Delimiter">The delimiter of columns. Defaults to ','.</param>
/// <param name="Mapping">A mapping for converting raw CSV values to a CSV record.</param>
public record CsvOptions<T>(
    ICsvMapping<T> Mapping,
    int? CodePage = null,
    char Delimiter = ',')
    : CsvOptions(
        CodePage,
        Delimiter,
        HasHeaderLine: Mapping is CsvHeaderMapping<T>,
        Mapping.ColumnNames);