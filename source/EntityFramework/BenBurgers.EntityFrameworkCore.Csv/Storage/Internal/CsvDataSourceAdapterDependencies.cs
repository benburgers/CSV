/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <summary>
/// Dependencies for a CSV data source adapter.
/// </summary>
/// <param name="Options">
/// The CSV configuration options.
/// </param>
/// <param name="ModelSource">
/// The model source.
/// </param>
internal sealed record CsvDataSourceAdapterDependencies(ICsvSingletonOptions Options, IModelSource ModelSource);
