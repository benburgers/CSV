# Entity Framework Core extension for CSV data

## Introduction

This package is an Entity Framework Core extension for CSV data.
It enables the implementer to read and write CSV data to configured data sources.
The features in this package provide the means to map CSV data to columns, data sources, query and synchronize the data.

By using this extension, CSV data will work seamlessly with other Entity Framework Core extensions.

Most of the source code for this package has been inspired by the Entity Framework Core extension for Cosmos, in the official Entity Framework Core repository.

## Configuration

As per convention, the CSV extension can be configured by using the `UseCsv` method on the `DbContextOptionsBuilder`.

```csharp
optionsBuilder.UseCsv<MyDbContext>(new DirectoryInfo("C:\\Demo\\CSV\\"));
```

If using the Dependency Injection features from Microsoft, the `AddCsv` extension method:

```csharp
serviceCollection.AddCsv<MyDbContext>(new DirectoryInfo("C:\\Demo\\CSV\\"));
```

An additional builder action offers the ability to configure more.

## Entity Type Builder

The entity type builder has an extension method `ToCsvDataSource` to determine the location of a particular entity's CSV data.
This is an optional configuration; without it an entity automatically maps to a CSV file with the name of the entity in the directory configured in the database context.

## Model Builder

The model builder has an extension method `HasCsvDataSourceDefault` to determine a default data source for entities.
This is an optional configuration; without it an entity automatically maps to a CSV file with the name of the entity in the directory configured in the database context.

## Property Builder

The property builder has an extension method `HasCsvColumn` to indicate the CSV column to which a property is mapped.
This designation is optional, since the CSV data may have a header line with column names, 
or the order of an entity's properties and their names may determine the mapping automatically.