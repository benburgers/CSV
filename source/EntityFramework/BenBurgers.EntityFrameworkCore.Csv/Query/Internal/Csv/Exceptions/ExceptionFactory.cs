/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Exceptions;

internal static class ExceptionFactory
{
    internal static CsvTranslationException BinaryTranslationFailed(string? translationErrorDetails)
        => new(ExceptionMessages.BinaryTranslationFailed, translationErrorDetails);

    internal static CsvBindingException MemberBindingFailed(MemberIdentity memberIdentity)
        => new(memberIdentity);

    internal static CsvTranslationException MemberTranslationFailed(MemberInfo memberInfo, string? translationErrorDetails)
        => new(string.Format(ExceptionMessages.MemberTranslationFailed, memberInfo.Name), translationErrorDetails);
}
