/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// An expression that accesses a member.
/// </summary>
internal interface IAccessExpression
{
    /// <summary>
    /// Gets the name of the access symbol.
    /// </summary>
    string Name { get; }
}
