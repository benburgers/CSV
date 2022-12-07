/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Mapping.Exceptions;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// The default type converter for CSV readers and writers.
/// </summary>
internal sealed class CsvTypeConverterDefault : TypeConverter
{
    private static readonly IReadOnlyDictionary<Type, Func<string?, CultureInfo?, object?>> FromStringConverters =
        new Dictionary<Type, Func<string?, CultureInfo?, object?>>
        {
            { typeof(BigInteger?), (s, c) => s is null ? null : BigInteger.Parse(s, c) },
            { typeof(BigInteger), (s, c) => BigInteger.Parse(s!, c) },
            { typeof(bool?), (s, _) => s is null ? null : bool.Parse(s) },
            { typeof(bool), (s, _) => bool.Parse(s!) },
            { typeof(byte?), (s, c) => s is null ? null : byte.Parse(s, c) },
            { typeof(byte), (s, c) => byte.Parse(s!, c) },
            { typeof(char?), (s, _) => s is null ? null : char.Parse(s) },
            { typeof(char), (s, _) => char.Parse(s!) },
            { typeof(decimal?), (s, c) => s is null ? null : decimal.Parse(s, c) },
            { typeof(decimal), (s, c) => decimal.Parse(s!, c) },
            { typeof(double?), (s, c) => s is null ? null : double.Parse(s, c) },
            { typeof(double), (s, c) => double.Parse(s!, c) },
            { typeof(float?), (s, c) => s is null ? null : float.Parse(s, c) },
            { typeof(float), (s, c) => float.Parse(s!, c) },
            { typeof(int?), (s, c) => s is null ? null : int.Parse(s, c) },
            { typeof(int), (s, c) => int.Parse(s!, c) },
            { typeof(long?), (s, c) => s is null ? null : long.Parse(s, c) },
            { typeof(long), (s, c) => long.Parse(s!, c) },
            { typeof(short?), (s, c) => s is null ? null : short.Parse(s, c) },
            { typeof(short), (s, c) => short.Parse(s!, c) },
            { typeof(string), (s, _) => s }
        };

    private static readonly IReadOnlyDictionary<Type, Func<object?, CultureInfo?, string?>> ToStringConverters =
        new Dictionary<Type, Func<object?, CultureInfo?, string?>>
        {
            { typeof(BigInteger?), (o, c) => ((BigInteger?)o)?.ToString(c) },
            { typeof(BigInteger), (o, c) => ((BigInteger)o!).ToString(c)! },
            { typeof(bool?), (o, _) => ((bool?)o)?.ToString() },
            { typeof(bool), (o, _) => ((bool)o!).ToString()! },
            { typeof(byte?), (o, c) => ((byte?)o)?.ToString(c) },
            { typeof(byte), (o, c) => ((byte)o!).ToString(c)! },
            { typeof(char?), (o, c) => ((char?)o)?.ToString(c) },
            { typeof(char), (o, c) => ((char)o!).ToString(c)! },
            { typeof(decimal?), (o, c) => ((decimal?)o)?.ToString(c) },
            { typeof(decimal), (o, c) => ((decimal)o!).ToString(c)! },
            { typeof(double?), (o, c) => ((double?)o)?.ToString(c) },
            { typeof(double), (o, c) => ((double)o!).ToString(c)! },
            { typeof(float?), (o, c) => ((float?)o)?.ToString(c) },
            { typeof(float), (o, c) => ((float)o!).ToString(c)! },
            { typeof(int?), (o, c) => ((int?)o)?.ToString(c) },
            { typeof(int), (o, c) => ((int)o!).ToString(c)! },
            { typeof(long?), (o, c) => ((long?)o)?.ToString(c) },
            { typeof(long), (o, c) => ((long)o!).ToString(c)! },
            { typeof(short?), (o, c) => ((short?)o)?.ToString(c) },
            { typeof(short), (o, c) => ((short)o!).ToString(c)! },
            { typeof(string), (o, _) => (string?)o }
        };

    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return
            sourceType == typeof(string)
            || ToStringConverters.ContainsKey(sourceType);
    }

    /// <inheritdoc />
    /// <exception cref="CsvTypeConverterDoesNotSupportSourceTypeException">
    /// A <see cref="CsvTypeConverterDoesNotSupportSourceTypeException" /> is thrown if the source type is not supported.
    /// </exception>
    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value)
    {
        var sourceType = value.GetType();
        if (!ToStringConverters.TryGetValue(sourceType, out var converter))
            throw new CsvTypeConverterDoesNotSupportSourceTypeException(sourceType);
        return converter(value, culture);
    }

    /// <inheritdoc />
    public override bool CanConvertTo(
        ITypeDescriptorContext? context,
        Type? destinationType)
    {
        return
            destinationType is not null
            && (destinationType == typeof(string)
            || FromStringConverters.ContainsKey(destinationType));
    }

    /// <inheritdoc />
    /// <exception cref="CsvTypeConverterValueMustBeStringException">
    /// A <see cref="CsvTypeConverterValueMustBeStringException" /> is thrown if the source value is not a <see cref="string" />.
    /// </exception>
    /// <exception cref="CsvTypeConverterDoesNotSupportDestinationTypeException">
    /// A <see cref="CsvTypeConverterDoesNotSupportDestinationTypeException" /> is thrown if the destination type is not supported.
    /// </exception>
    public override object? ConvertTo(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value,
        Type destinationType)
    {
        if (value is not string)
            throw new CsvTypeConverterValueMustBeStringException();
        if (!FromStringConverters.TryGetValue(destinationType, out var converter))
            throw new CsvTypeConverterDoesNotSupportDestinationTypeException(destinationType);
        return converter((string?)value, culture);
    }
}
