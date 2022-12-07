/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata;

/// <summary>
/// A CSV column.
/// </summary>
/// <param name="ColumnIndex">An optional fixed index for the column.</param>
/// <param name="ColumnName">An optional (alternative) name for the column.</param>
public sealed record CsvColumn(int? ColumnIndex = null, string? ColumnName = null);