/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using System.Text;

namespace BenBurgers.Text.Csv.Tests.CsvStreams;

public partial class CsvStreamTests
{
    public static readonly IEnumerable<object?[]> InsertLineParameters =
        new[]
        {
            new object?[]
            {
                "Sample",
                new CsvOptions(),
                1L,
                new string[] { "ABC", "DEF", "123", "456", "GHI", "JKL", "789", "MNO", "PQR" },
@"1,2,3,4,5,6,7,8,9
ABC,DEF,123,456,GHI,JKL,789,MNO,PQR
9,8,7,6,5,4,3,2,1
a,b,c,d,e,f,g,h,i
i,h,g,f,e,d,c,b,a
"
            },
            new object?[]
            {
                "Sample_header",
                new CsvOptions(HasHeaderLine: true),
                1L,
                new string[] { "ABC", "DEF", "123", "456", "GHI", "JKL", "789", "MNO", "PQR" },
@"Abc,Def,Ghi,Jkl,Mno,Pqr,Stu,Vwx,Yz
1,2,3,4,5,6,7,8,9
ABC,DEF,123,456,GHI,JKL,789,MNO,PQR
9,8,7,6,5,4,3,2,1
a,b,c,d,e,f,g,h,i
i,h,g,f,e,d,c,b,a
"
            }
        };

    [Theory(DisplayName = "CSV Stream :: InsertLine")]
    [MemberData(nameof(InsertLineParameters))]
    public void InsertLineTest(
        string sampleName,
        CsvOptions csvOptions,
        long lineNumber,
        string[] values,
        string expected)
    {
        // Arrange
        using var sampleStream = GetSample(sampleName);
        using var testStream = new MemoryStream();
        sampleStream.CopyTo(testStream);
        testStream.Seek(0L, SeekOrigin.Begin);
        using var csvStream = new CsvStream(testStream, csvOptions);

        // Act
        csvStream.InsertLine(lineNumber, values);
        csvStream.Flush();

        // Assert
        var data = testStream.ToArray();
        var actual = Encoding.UTF8.GetString(data);
        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "CSV Stream :: InsertLineAsync")]
    [MemberData(nameof(InsertLineParameters))]
    public async Task InsertLineTestAsync(
        string sampleName,
        CsvOptions csvOptions,
        long lineNumber,
        string[] values,
        string expected)
    {
        // Arrange
        using var sampleStream = GetSample(sampleName);
        using var testStream = new MemoryStream();
        await sampleStream.CopyToAsync(testStream);
        testStream.Seek(0L, SeekOrigin.Begin);
        using var csvStream = new CsvStream(testStream, csvOptions);

        // Act
        await csvStream.InsertLineAsync(lineNumber, values);
        await csvStream.FlushAsync();

        // Assert
        var data = testStream.ToArray();
        var actual = Encoding.UTF8.GetString(data);
        Assert.Equal(expected, actual);
    }
}
