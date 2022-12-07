/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using System.Linq.Expressions;
using System.Reflection;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;

/// <summary>
/// A member expression on a CSV entity.
/// </summary>
internal sealed partial class CsvMemberExpression : CsvExpression
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvMemberExpression" />.
    /// </summary>
    /// <param name="instance">The member instance.</param>
    /// <param name="memberInfo">The member info.</param>
    /// <param name="returnType">The member return type.</param>
    internal CsvMemberExpression(CsvExpression instance, MemberInfo memberInfo, Type returnType)
        : base(returnType, instance.TypeMapping)
    {
        this.Instance = instance;
        this.MemberInfo = memberInfo;
        this.ReturnType = returnType;
    }

    /// <summary>
    /// Gets the member instance.
    /// </summary>
    internal CsvExpression Instance { get; }

    /// <summary>
    /// Gets the member info.
    /// </summary>
    internal MemberInfo MemberInfo { get; }

    /// <summary>
    /// Gets the return type.
    /// </summary>
    internal Type ReturnType { get; }

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor)
        => this;
}
