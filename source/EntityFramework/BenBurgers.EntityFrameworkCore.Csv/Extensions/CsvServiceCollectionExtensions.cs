/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Diagnostics.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Infrastructure;
using BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Metadata.Conventions.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Query.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;
using BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace BenBurgers.EntityFrameworkCore.Csv.Extensions;

/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class CsvServiceCollectionExtensions
{
    /// <summary>
    /// Registers the given Entity Framework <see cref="DbContext" /> as a service in <paramref name="serviceCollection" /> 
    /// and configures it to read and write CSV data.
    /// </summary>
    /// <typeparam name="TContext">The type of context to be registered.</typeparam>
    /// <param name="serviceCollection">The services for dependency injection.</param>
    /// <param name="directoryDefault">The default directory that contains CSV files.</param>
    /// <param name="csvOptionsAction">An optional action to allow additional CSV-specific configuration.</param>
    /// <param name="optionsAction">An optional action to configure the <see cref="DbContextOptions" /> for the context.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddCsv<TContext>(
        this IServiceCollection serviceCollection,
        DirectoryInfo directoryDefault,
        Action<CsvDbContextOptionsBuilder>? csvOptionsAction = null,
        Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
        => serviceCollection.AddDbContext<TContext>(
            (serviceProvider, options) =>
            {
                optionsAction?.Invoke(options);
                options.UseCsv(directoryDefault, csvOptionsAction);
            });

    /// <summary>
    /// Adds Entity Framework Core services using CSV data.
    /// </summary>
    /// <param name="serviceCollection">The services for dependency injection.</param>
    /// <returns>The services for dependency injection.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IServiceCollection AddEntityFrameworkCsv(this IServiceCollection serviceCollection)
    {
        var builder =
            new EntityFrameworkServicesBuilder(serviceCollection)
                .TryAdd<IDatabase, CsvDatabaseWrapper>()
                .TryAdd<IDatabaseCreator, CsvDatabaseCreator>()
                .TryAdd<IDatabaseProvider, DatabaseProvider<CsvOptionsExtension>>()
                .TryAdd<IDbContextTransactionManager, CsvDbContextTransactionManager>()
                .TryAdd<LoggingDefinitions, CsvLoggingDefinitions>()
                .TryAdd<IModelValidator, CsvModelValidator>()
                .TryAdd<IProviderConventionSetBuilder, CsvConventionSetBuilder>()
                .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, CsvQueryableMethodTranslatingExpressionVisitorFactory>()
                .TryAdd<IQueryCompilationContextFactory, CsvQueryCompilationContextFactory>()
                .TryAdd<IQueryContextFactory, CsvQueryContextFactory>()
                .TryAdd<IShapedQueryCompilingExpressionVisitorFactory, CsvShapedQueryCompilingExpressionVisitorFactory>()
                .TryAdd<ISingletonOptions, ICsvSingletonOptions>(sp => sp.GetRequiredService<ICsvSingletonOptions>())
                .TryAdd<ITypeMappingSource, CsvTypeMappingSource>()
                .TryAddProviderSpecificServices(
                    b => b
                        .TryAddSingleton(sp => new CsvDataSourceAdapterDependencies(sp.GetRequiredService<ICsvSingletonOptions>(), sp.GetRequiredService<IModelSource>()))
                        .TryAddSingleton<ICsvDataSourceAdapterFactory, CsvDataSourceAdapterFactory>()
                        .TryAddSingleton<ICsvSingletonOptions, CsvSingletonOptions>()
                        .TryAddScoped<ICsvExpressionFactory, CsvExpressionFactory>());

        builder.TryAddCoreServices();

        return serviceCollection;
    }
}
