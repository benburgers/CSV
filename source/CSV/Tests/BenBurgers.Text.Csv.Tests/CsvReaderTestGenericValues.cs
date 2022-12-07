/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Mapping;
using System.Collections;

namespace BenBurgers.Text.Csv.Tests;

public class CsvReaderTestGenericValues : IEnumerable<object?[]>
{
    public sealed record MockCsvRecord(string Value1, int Value2, string Value3, string Value4);

    private static readonly IEnumerable<object?[]> Values =
        new[]
        {
            new object?[]
            {
@"abc,123,ABC,!@#
def,456,DEF,$%^
",
                new CsvOptions<MockCsvRecord>(new CsvConverterMapping<MockCsvRecord>(
                    record => new string[] { record.Value1, record.Value2.ToString(), record.Value3, record.Value4 },
                    rawValues => new(rawValues[0], int.Parse(rawValues[1]), rawValues[2], rawValues[3]))),
                new[]
                {
                    new MockCsvRecord("abc", 123, "ABC", "!@#"),
                    new MockCsvRecord("def", 456, "DEF", "$%^")
                }
            },
            new object?[]
            {
@"Value1,Value2,Value3,Value4
abc,123,ABC,!@#
def,456,DEF,$%^
",
                new CsvOptions<MockCsvRecord>(new CsvHeaderMapping<MockCsvRecord>(
                    rawValues => new(
                        rawValues[nameof(MockCsvRecord.Value1)],
                        int.Parse(rawValues[nameof(MockCsvRecord.Value2)]),
                        rawValues[nameof(MockCsvRecord.Value3)],
                        rawValues[nameof(MockCsvRecord.Value4)]),
                    new Dictionary<string, Func<MockCsvRecord, string?>>
                    {
                        { nameof(MockCsvRecord.Value1), m => m.Value1 },
                        { nameof(MockCsvRecord.Value2), m => m.Value2.ToString() },
                        { nameof(MockCsvRecord.Value3), m => m.Value3 },
                        { nameof(MockCsvRecord.Value4), m => m.Value4 }
                    })
                ),
                new[]
                {
                    new MockCsvRecord("abc", 123, "ABC", "!@#"),
                    new MockCsvRecord("def", 456, "DEF", "$%^")
                }
            },
            new object?[] {
@"Value1,Value2,Value3,Value4
abc,123,ABC,!@#
def,456,DEF,$%^
",
                new CsvOptions<MockCsvRecord>(new CsvHeaderTypeMapping<MockCsvRecord>()),
                new[]
                {
                    new MockCsvRecord("abc", 123, "ABC", "!@#"),
                    new MockCsvRecord("def", 456, "DEF", "$%^")
                }
            },
            new object?[]
            {
@"abc;123;ABC;!@#
def;456;DEF;$%^
",
                new CsvOptions<MockCsvRecord>(new CsvConverterMapping<MockCsvRecord>(
                    record => new string[] { record.Value1, record.Value2.ToString(), record.Value3, record.Value4 },
                    rawValues => new(rawValues[0], int.Parse(rawValues[1]), rawValues[2], rawValues[3])),
                    Delimiter: ';'),
                new[]
                {
                    new MockCsvRecord("abc", 123, "ABC", "!@#"),
                    new MockCsvRecord("def", 456, "DEF", "$%^")
                }
            }
        };

    public IEnumerator<object?[]> GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
