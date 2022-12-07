/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;

internal sealed partial class CsvMemberExpression
{
    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            _ when ReferenceEquals(this, obj) => true,
            CsvMemberExpression memberExpression => this.Equals(memberExpression),
            _ => false
        };
    }

    private bool Equals(CsvMemberExpression other)
        => base.Equals(other)
            && this.Instance.Equals(other.Instance)
            && this.MemberInfo.Equals(other.MemberInfo)
            && this.ReturnType.Equals(other.ReturnType);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), this.Instance, this.MemberInfo, this.ReturnType);
}
