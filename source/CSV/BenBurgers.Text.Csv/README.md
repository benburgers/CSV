# CSV

This package provides features for reading from and writing to CSV data.

## CSV Reader

The CSV Reader can read CSV data from a data stream.
The simple implementation only returns an array of raw data, whereas the generic implementation maps data to a predefined CSV record.

### Raw

#### Construction

The simple CSV Reader can easily be setup like so:

```csharp
using BenBurgers.Text.Csv;

var options = new CsvOptions(Delimiter: ';');
using var reader = new CsvReader(stream, options);
```

It uses the delimiter character `;` for separating column values.
The `CsvOptions` also has a property that indicates whether the CSV data source has a header line with column names.

```csharp
using BenBurgers.Text.Csv;

var options = new CsvOptions(Delimiter: ';', HasHeaderLine: true);
using var reader = new CsvReader(stream, options);
```

In this case the reader automatically reads the first line and sets the `ColumnNames` property with the names read on the first line.
Additionally, a `CodePage` may be configured to identify the encoding of the file. This defaults to `UTF-8`.

#### Reading data

The raw values may be read line by line:

```csharp
var rawValues = await reader.ReadLineAsync(); // returns IReadOnlyList<string>
```

An optional `CancellationToken` may be passed as a parameter.

### Generic

The generic CSV Reader has an additional option to map CSV raw data.
The `BenBurgers.Text.Csv.Mapping` namespace has mapping features.

There are two ways (one is extended) for mapping:

#### Converter

The converter gets a function that receives the raw data as strings and outputs the CSV record.
The implementer has full control over how the raw values are converted to a record.

```csharp
using BenBurgers.Text.Csv;

var converterMapping = new CsvConverterMapping<MyCsvRecord>(rawValues => new MyCsvRecord(DoSomething(rawValues[0]), DoSomething2(rawValues[1])));
var options = new CsvOptions<MyCsvRecord>(converterMapping);
using var reader = new CsvReader(stream, options);
```

#### Header

The CSV data has a header line with column names. These names are then used to infer to which property or constructor parameter the values will be mapped.

```csharp
using BenBurgers.Text.Csv;

var headerMapping =
	new CsvHeaderMapping<MyCsvRecord>(
		new[] { "SomeProperty", "AnotherProperty" },
		rawValues => new(
			rawValues[nameof(MyCsvRecord.SomeProperty)],
			DoSomething(rawValues[nameof(MyCsvRecord.AnotherProperty)])),
		new Dictionary<string, Func<MyCsvRecord, string>>
		{
			{ nameof(MyCsvRecord.SomeProperty), m => m.SomeProperty },
			{ nameof(MyCsvRecord.AnotherProperty), m => DoSomething(m.AnotherProperty) }
		});
var options = new CsvOptions<MyCsvRecord>(headerMapping);
using var reader = new CsvReader(stream, options);
```

This approach presumes the CSV data has a header line.
A specialization is a mapping that uses reflection to automatically determine which column maps to which property or constructor parameter.

```csharp
using BenBurgers.Text.Csv;

var headerTypeMapping = new CsvHeaderTypeMapping<MyCsvRecord>();
var options = new CsvOptions<MyCsvRecord>(headerTypeMapping);
using var reader = new CsvReader(stream, options);
```

An optional `TypeConverter` may be passed to the `CsvHeaderTypeMapping<>`'s constructor for converting properties' and constructor parameters' values to the raw values and vice versa.

## CSV Writer

Conversely, the CSV Writer accepts a stream and configuration options as well.

### Raw

```csharp
using BenBurgers.Text.Csv;

var options = new CsvOptions();
using var writer = new CsvWriter(stream, options);
await writer.WriteLineAsync(new [] { "Value1", "123" });
```

### Generic

```csharp
using BenBurgers.Text.Csv;

var options = new CsvOptions<MyCsvRecord>(mapping);
using var writer = new CsvWriter<MyCsvRecord>(stream, options);
var record = new MyCsvRecord("Value1", 123);
await writer.WriteLineAsync(record);
```

## CSV Stream

A CSV Stream is a combination of a reader and a writer. It also provides additional functionality such as looking up a particular line or inserting/appending a line to CSV data.

```csharp
using BenBurgers.Text.Csv;

var options new CsvOptions();
using var stream = new CsvStream(stream, options);
stream.GoTo(5L); // Go to the 6th line (zero-based)
var values = stream.ReadLine(); // Returns the CSV values at the 6th line.
stream.InsertLine(3L, new[] { "foo", "123", "bar" }); // Inserts the specified values at the 4th line (zero-based).
stream.AppendLine(new[] { "bar", "321", "foo" }); // Appends the specified values at the end of the stream.
```

The generic CSV Stream has similar features to the generic reader and generic writer and uses the same mapping options.

## CSV Stream Factory

A CSV Stream Factory facilitates creating CSV streams from various sources.

- A `FileInfo`.
- An `HttpClient`.
- A `Stream`.
- A file located at a particular path.

```csharp
using BenBurgers.Text.Csv;

var options = new CsvOptions();
var factory = new CsvStreamFactory(options);
using var csvStream = factory.FromFile("C:\\foo\\bar.csv");
```

```csharp
using BenBurgers.Text.Csv;

var options = new CsvOptions();
var factory = new CsvStreamFactory(options);
using var memoryStream = new MemoryStream();
using var csvStream = factory.From(memoryStream);
```