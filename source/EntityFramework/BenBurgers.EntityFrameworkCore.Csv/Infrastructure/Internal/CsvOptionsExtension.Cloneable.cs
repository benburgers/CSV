/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;

internal sealed partial class CsvOptionsExtension
{
    /// <inheritdoc />
    object ICloneable.Clone() => this.Clone();

    /// <summary>
    /// Returns a clone of this instance.
    /// </summary>
    /// <returns>The clone of this instance.</returns>
    private CsvOptionsExtension Clone() => new(this); 
}
