/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;

/// <summary>
/// Validates models for CSV data.
/// </summary>
internal sealed class CsvModelValidator : ModelValidator
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvModelValidator" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    public CsvModelValidator(ModelValidatorDependencies dependencies)
        : base(dependencies)
    {
    }

    public override void Validate(IModel model, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
        base.Validate(model, logger);
    }
}
