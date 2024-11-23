/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Tests.Resources;
using System.Text;

namespace BenBurgers.Text.Csv.Tests.CsvStreams;

public partial class CsvStreamTests
{
    public static readonly IEnumerable<object?[]> AppendLineParameters =
        new[]
        {
            new object?[]
            {
                nameof(CsvSamples.Sample),
                new CsvOptions(),
                new [] { "abc", "123", "def", "456", "ghi", "789", "jkl", "012", "mno" },
@"1,2,3,4,5,6,7,8,9
9,8,7,6,5,4,3,2,1
a,b,c,d,e,f,g,h,i
i,h,g,f,e,d,c,b,a
abc,123,def,456,ghi,789,jkl,012,mno
"
            },
            new object?[]
            {
                nameof(CsvSamples.Sample_header),
                new CsvOptions(),
                new [] { "abc", "123", "def", "456", "ghi", "789", "jkl", "012", "mno" },
@"Abc,Def,Ghi,Jkl,Mno,Pqr,Stu,Vwx,Yz
1,2,3,4,5,6,7,8,9
9,8,7,6,5,4,3,2,1
a,b,c,d,e,f,g,h,i
i,h,g,f,e,d,c,b,a
abc,123,def,456,ghi,789,jkl,012,mno
"
            }
        };

    [Theory(DisplayName = "CSV Stream :: AppendLine")]
    [MemberData(nameof(AppendLineParameters))]
    public void AppendLineTest(
        string sampleName,
        CsvOptions csvOptions,
        string[] values,
        string expected)
    {
        // Arrange
        using var sampleStream = GetSample(sampleName);
        using var testStream = new MemoryStream();
        sampleStream.CopyTo(testStream);
        using var csvStream = new CsvStream(testStream, csvOptions);

        // Act
        csvStream.AppendLine(values);
        csvStream.Flush();

        // Assert
        var data = testStream.ToArray();
        var actual = Encoding.UTF8.GetString(data);
        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "CSV Stream :: AppendLineAsync")]
    [MemberData(nameof(AppendLineParameters))]
    public async Task AppendLineTestAsync(
        string sampleName,
        CsvOptions csvOptions,
        string[] values,
        string expected)
    {
        // Arrange
        var sampleStream = GetSample(sampleName);
        var testStream = new MemoryStream();
        await sampleStream.CopyToAsync(testStream);
        var csvStream = new CsvStream(testStream, csvOptions);

        // Act
        await csvStream.AppendLineAsync(values);
        await csvStream.FlushAsync();

        // Assert
        var data = testStream.ToArray();
        var actual = Encoding.UTF8.GetString(data);
        Assert.Equal(expected, actual);
    }
}
