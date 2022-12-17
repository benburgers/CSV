/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Attributes;
using BenBurgers.Text.Csv.Mapping.Exceptions;
using System.ComponentModel;
using System.Reflection;

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// Maps a type's reflection to columns in CSV data based on the column names in its header.
/// </summary>
/// <typeparam name="T">The CSV record type.</typeparam>
public sealed class CsvHeaderTypeMapping<T> : CsvHeaderMapping<T>
{
    private sealed record ConstructorMapping(
        ConstructorInfo ConstructorInfo,
        IReadOnlyList<string> ColumnNames,
        Func<IReadOnlyDictionary<string, string>, IReadOnlyList<object?>> Transformer);

    private static readonly IReadOnlyList<ConstructorInfo> Constructors =
        typeof(T)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .ToArray();

    private static readonly IReadOnlyList<PropertyInfo> Properties =
        typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

    private static readonly IReadOnlyList<PropertyInfo> PropertiesWithPublicSetters =
        Properties
            .Where(p => p.SetMethod is { IsPublic: true })
            .ToArray();

    private static readonly CsvTypeConverterDefault TypeConverterDefault = new();

    /// <summary>
    /// Initializes a new instance of <see cref="CsvHeaderMapping{T}" />.
    /// </summary>
    /// <exception cref="CsvHeaderTypeMappingNoSuitableConstructorFoundException">
    /// A <see cref="CsvHeaderTypeMappingNoSuitableConstructorFoundException" /> is thrown if no suitable constructor is found on the CSV record that can be mapped.
    /// </exception>
    public CsvHeaderTypeMapping(TypeConverter? typeConverter = null)
        : base(
            GetColumnNames(),
            CreateConsumer(typeConverter ?? TypeConverterDefault),
            CreateProducer(typeConverter ?? TypeConverterDefault))
    {
        this.TypeConverter = typeConverter ?? TypeConverterDefault;
    }

    /// <summary>
    /// Gets the type converter.
    /// </summary>
    public TypeConverter TypeConverter { get; }

    private static ConstructorMapping CreateConstructorMapping(ConstructorInfo constructorInfo, TypeConverter typeConverter)
    {
        var parameters = constructorInfo.GetParameters();
        var parameterMappings =
            parameters
                .Select(
                    p =>
                    p.GetCustomAttribute<CsvColumnAttribute>() is { Name: { } parameterColumnName }
                        ? (p.ParameterType, ColumnName: parameterColumnName)
                        : PropertiesWithPublicSetters
                            .FirstOrDefault(py => py.Name == p.Name)?
                            .GetCustomAttribute<CsvColumnAttribute>() is { Name: { } propertyColumnName }
                                ? (p.ParameterType, ColumnName: propertyColumnName)
                                : (p.ParameterType, ColumnName: p.Name!))
                .ToArray();
        var columnNames =
            parameterMappings
                .Select(pm => pm.ColumnName)
                .ToArray();
        var transformer = new Func<IReadOnlyDictionary<string, string>, IReadOnlyList<object?>>(
            v =>
            parameterMappings
                .Select(
                    pm =>
                    v.TryGetValue(pm.ColumnName, out var rawValue)
                        ? typeConverter.ConvertTo(rawValue, pm.ParameterType)
                        : throw new CsvHeaderTypeMappingNoSuitableConstructorFoundException())
                .ToArray());
        return new ConstructorMapping(constructorInfo, columnNames, transformer);
    }

    private static Func<IReadOnlyDictionary<string, string>, T> CreateConsumer(TypeConverter typeConverter)
    {
        var constructorMappings =
            Constructors
                .Select(c => CreateConstructorMapping(c, typeConverter))
                .ToArray();
        ConstructorMapping? constructorMapping = null;
        var propertyColumnNameMappings =
            PropertiesWithPublicSetters
                .ToDictionary(
                    p => p,
                    p =>
                        p.GetCustomAttribute<CsvColumnAttribute>() is { } csvColumnAttribute
                            ? csvColumnAttribute.Name
                            : p.Name!);

        return rawValues =>
        {
            // Create instance.
            var columnNames = rawValues.Keys.ToArray();
            if (constructorMapping is not { } map)
            {
                constructorMapping =
                    constructorMappings
                        .Where(cm => columnNames.All(cn => cm.ColumnNames.Contains(cn)))
                        .OrderByDescending(cm => cm.ColumnNames.Count)
                        .FirstOrDefault() ?? throw new CsvHeaderTypeMappingNoSuitableConstructorFoundException();
            }
            var constructorParameters = constructorMapping.Transformer(rawValues);
            var instance = (T)constructorMapping.ConstructorInfo.Invoke(constructorParameters.ToArray());

            // Hydrate properties.
            foreach (var property in PropertiesWithPublicSetters)
            {
                if (rawValues.TryGetValue(propertyColumnNameMappings[property], out var value))
                    property.SetValue(instance, typeConverter.ConvertTo(value, property.PropertyType));
            }

            return instance;
        };
    }

    private static IReadOnlyDictionary<string, Func<T, string?>> CreateProducer(TypeConverter typeConverter) =>
        Properties
            .ToDictionary(
                p => p.GetCustomAttribute<CsvColumnAttribute>() is { Name: { } columnName } ? columnName : p.Name,
                p => new Func<T, string?>(o => (string?)typeConverter.ConvertFrom(p.GetValue(o)!)));

    private static IReadOnlySet<string> GetColumnNames() =>
        PropertiesWithPublicSetters
            .Select(p => p.GetCustomAttribute<CsvColumnAttribute>() is { Name: { } columnName }
                ? columnName
                : p.Name)
            .ToArray()
            .ToHashSet();
}
