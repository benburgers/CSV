/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <inheritdoc />
internal sealed class CsvDataSourceAdapterFactory : ICsvDataSourceAdapterFactory
{
    private static readonly IReadOnlyDictionary<Type, Func<CsvDataSourceAdapterDependencies, CsvDataSource, ICsvDataSourceAdapter>> mapping =
        new Dictionary<Type, Func<CsvDataSourceAdapterDependencies, CsvDataSource, ICsvDataSourceAdapter>>
        {
            { typeof(CsvFileSource), (dependencies, dataSource) => new CsvFileSourceAdapter(dependencies, (CsvFileSource)dataSource) }
        };

    /// <summary>
    /// Initializes a new instance of <see cref="CsvDataSourceAdapterFactory" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    public CsvDataSourceAdapterFactory(CsvDataSourceAdapterDependencies dependencies)
    {
        this.Dependencies = dependencies;
    }

    /// <summary>
    /// Gets the dependencies.
    /// </summary>
    internal CsvDataSourceAdapterDependencies Dependencies { get; }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">
    /// A <see cref="NotSupportedException" /> is thrown if <paramref name="dataSource" /> is not supported.
    /// </exception>
    public ICsvDataSourceAdapter Create(CsvDataSource dataSource)
    {
        if (!mapping.TryGetValue(dataSource.GetType(), out var adapterFactory))
        {
            throw new NotSupportedException();
        }
        return adapterFactory(this.Dependencies, dataSource);
    }
}
