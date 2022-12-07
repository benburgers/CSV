/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// An expression that provides access to an object.
/// </summary>
internal sealed class ObjectAccessExpression : Expression, IAccessExpression
{
    /// <summary>
    /// Initializes a new instance of <see cref="ObjectAccessExpression" />.
    /// </summary>
    /// <param name="navigation">The navigation.</param>
    /// <param name="accessExpression">The access expression.</param>
    /// <exception cref="NotSupportedException">Navigations are not supported in CSV.</exception>
    internal ObjectAccessExpression(INavigation navigation, Expression accessExpression)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Gets the name of the object access.
    /// </summary>
    /// <exception cref="NotSupportedException">Navigations are not supported in CSV.</exception>
    public string Name => throw new NotSupportedException();
}
