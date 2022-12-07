/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Conventions.Internal;

internal sealed class CsvConventionSetBuilder : ProviderConventionSetBuilder
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvConventionSetBuilder" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    public CsvConventionSetBuilder(ProviderConventionSetBuilderDependencies dependencies)
        : base(dependencies)
    {
    }

    public override ConventionSet CreateConventionSet()
    {
        var conventionSet = base.CreateConventionSet()!;

        conventionSet.ModelFinalizingConventions.Add(new CsvColumnConvention());

        return conventionSet;
    }
}
