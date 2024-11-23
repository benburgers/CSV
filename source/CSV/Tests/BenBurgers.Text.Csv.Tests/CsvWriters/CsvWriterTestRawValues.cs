/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using System.Collections;

namespace BenBurgers.Text.Csv.Tests.CsvWriters;

public class CsvWriterTestRawValues : IEnumerable<object?[]>
{
    private static readonly IEnumerable<object?[]> Values =
        new[]
        {
            new object?[]
            {
                new CsvOptions(),
                new string[][]
                {
                    new[] { "abc", "123", "ABC", "!@#" },
                    new[] { "def", "456", "DEF", "$%^" }
                },
@"abc,123,ABC,!@#
def,456,DEF,$%^
"
            },
            new object?[]
            {
                new CsvOptions(Delimiter: ';'),
                new string[][]
                {
                    new[] { "abc", "123", "ABC", "!@#" },
                    new[] { "def", "456", "DEF", "$%^" }
                },
@"abc;123;ABC;!@#
def;456;DEF;$%^
"
            }
        };

    public IEnumerator<object?[]> GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
