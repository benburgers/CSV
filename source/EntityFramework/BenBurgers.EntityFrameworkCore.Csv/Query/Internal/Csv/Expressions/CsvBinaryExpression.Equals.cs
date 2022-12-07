/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

internal sealed partial class CsvBinaryExpression
{
    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            _ when ReferenceEquals(this, obj) => true,
            CsvBinaryExpression expression => this.Equals(expression),
            _ => false
        };
    }

    /// <summary>
    /// Determines whether the specified <see cref="CsvBinaryExpression" /> is equal to the current <see cref="CsvBinaryExpression" />.
    /// </summary>
    /// <param name="other">The <see cref="CsvBinaryExpression" /> to compare with the current <see cref="CsvBinaryExpression" />.</param>
    /// <returns><see langword="true" /> if the specified <see cref="CsvBinaryExpression" /> is equal to the current <see cref="CsvBinaryExpression" />; otherwise, <see langword="false" />.</returns>
    private bool Equals(CsvBinaryExpression other)
        => base.Equals(other)
            && this.OperatorType.Equals(other.OperatorType)
            && this.Left.Equals(other.Left)
            && this.Right.Equals(other.Right);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), this.OperatorType, this.Left, this.Right);
}
