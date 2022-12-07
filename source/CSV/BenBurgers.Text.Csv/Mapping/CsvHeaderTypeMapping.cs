/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;
using System.ComponentModel;
using System.Reflection;

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// Maps a type's reflection to columns in CSV data based on the column names in its header.
/// </summary>
/// <typeparam name="T">The CSV record type.</typeparam>
public sealed class CsvHeaderTypeMapping<T> : CsvHeaderMapping<T>
{
    private static readonly CsvTypeConverterDefault TypeConverterDefault = new();

    private static readonly IReadOnlyList<ConstructorInfo> Constructors =
        typeof(T)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .ToArray();

    private static readonly IReadOnlyList<PropertyInfo> Properties =
        typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

    /// <summary>
    /// Initializes a new instance of <see cref="CsvHeaderMapping{T}" />.
    /// </summary>
    public CsvHeaderTypeMapping(TypeConverter? typeConverter = null)
        : base(
            FactoryFactory(typeConverter ?? TypeConverterDefault),
            CreateValueGetters(typeConverter ?? TypeConverterDefault))
    {
        this.TypeConverter = typeConverter ?? TypeConverterDefault;
    }

    /// <summary>
    /// Gets the type converter.
    /// </summary>
    public TypeConverter TypeConverter { get; }

    private static Func<IReadOnlyDictionary<string, string>, T> FactoryFactory(
        TypeConverter typeConverter)
    {
        return rawValues =>
        {
            // Create instance.
            var columnNames = rawValues.Keys.ToArray();
            var matchingConstructor =
                Constructors
                    .Select(c => (Constructor: c, Parameters: c.GetParameters()))
                    .Where(cp => cp.Parameters.Select(p => p.Name).All(n => columnNames.Contains(n)))
                    .OrderByDescending(cp => cp.Parameters.Length)
                    .Select(cp => cp.Constructor)
                    .FirstOrDefault();
            if (matchingConstructor is null)
            {
                throw new CsvException("");
            }
            var constructorParameters =
                matchingConstructor
                    .GetParameters()
                    .Select(p => typeConverter.ConvertTo(rawValues[p.Name!], p.ParameterType))
                    .ToArray();
            var instance = (T)Activator.CreateInstance(typeof(T), constructorParameters)!;

            // Hydrate properties.
            var propertiesWithSetters =
                Properties
                    .Where(p => p.SetMethod is { IsPublic: true })
                    .ToArray();
            foreach (var propertyWithSetter in propertiesWithSetters)
            {
                if (rawValues.TryGetValue(propertyWithSetter.Name, out var value))
                    propertyWithSetter.SetValue(instance, typeConverter.ConvertTo(value, propertyWithSetter.PropertyType));
            }

            return instance;
        };
    }

    private static IReadOnlyDictionary<string, Func<T, string?>> CreateValueGetters(
        TypeConverter typeConverter)
    {
        return
            Properties
                .ToDictionary(
                    p => p.Name,
                    p => new Func<T, string?>(o => (string?)typeConverter.ConvertFrom(p.GetValue(o)!)));
    }
}
