/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Query.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Exceptions;

/// <summary>
/// An exception that is thrown if a binding to a CSV record has failed.
/// </summary>
public sealed class CsvBindingException : CsvQueryException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvBindingException" />.
    /// </summary>
    /// <param name="memberIdentity">The member identity.</param>
    internal CsvBindingException(MemberIdentity memberIdentity)
        : base(CreateExceptionMessage(memberIdentity))
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvBindingException" />.
    /// </summary>
    /// <param name="property">The property.</param>
    internal CsvBindingException(IProperty property)
        : base(CreateExceptionMessage(property))
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvBindingException" />.
    /// </summary>
    /// <param name="navigation">The navigation.</param>
    internal CsvBindingException(INavigation navigation)
        : base(CreateExceptionMessage(navigation))
    {
    }

    private static string CreateExceptionMessage(MemberIdentity memberIdentity)
        => string.Format(ExceptionMessages.MemberBindingFailed, memberIdentity.Name ?? memberIdentity.MemberInfo?.Name ?? "?");

    private static string CreateExceptionMessage(IProperty property)
        => string.Format(ExceptionMessages.PropertyBindingFailed, property.Name);

    private static string CreateExceptionMessage(INavigation navigation)
        => string.Format(ExceptionMessages.NavigationBindingFailed, navigation.Name);
}
