/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Tests.Bootstrapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BenBurgers.Text.Csv.Tests.CsvStreamFactories;

[Collection(nameof(ConfigurationTestCollection))]
public class CsvStreamFactoryTests(ConfigurationTestFixture configurationTestFixture)
{
    [Fact(DisplayName = "CSV Stream Factory :: from FileInfo")]
    public void FromFileInfoTest()
    {
        // Arrange
        var configurationOptions =
            
                configurationTestFixture
                .ServiceProvider
                .GetRequiredService<IOptions<ConfigurationOptions>>()
                .Value;
        var testDirectoryName = configurationOptions.TestDirectoryName;
        var testFileName = Path.Combine(testDirectoryName, "TestFileInfo.csv");
        var testFileInfo = new FileInfo(testFileName);
        var options = new CsvOptions();
        var factory = new CsvStreamFactory(options);

        // Act
        using var stream = factory.From(testFileInfo);

        // Assert
        Assert.NotNull(stream);
        Assert.True(File.Exists(testFileName));
        stream.Dispose();
        File.Delete(testFileName);
    }

    [Fact(DisplayName = "CSV Stream Factory :: from HTTP Client")]
    public void FromHttpClientTest()
    {
        // Arrange
        const string SampleCsv =
@"Test1,Test2,Test3,Test4,Test5,Test6,Test7,Test8
testValue1,2,testValue3,4,testValue5,6,testValue7,7";
        var sampleCsvBytes = Encoding.UTF8.GetBytes(SampleCsv);

        var configurationOptions =
            
                configurationTestFixture
                .ServiceProvider
                .GetRequiredService<IOptions<ConfigurationOptions>>()
                .Value;

        var port = configurationOptions.ProxyPort;
        var hostIP = IPAddress.Parse("127.0.0.1");
        var httpRequestMessageExpected =
@$"GET /TestHttpClient.csv HTTP/1.1
Host: {hostIP}:{port}

";

        using var listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var hostEP = new IPEndPoint(hostIP, port);
        listeningSocket.Bind(hostEP);
        listeningSocket.Listen();
        var listenThread = new Thread(() =>
        {
            var connectionSocket = listeningSocket.Accept();
            var buffer = new byte[connectionSocket.ReceiveBufferSize];
            connectionSocket.Receive(buffer);
            var message = Encoding.UTF8.GetString(buffer);
            Assert.Equal(httpRequestMessageExpected, message[..message.IndexOf('\0')]);

            var responseBuilder = new StringBuilder();
            responseBuilder
                .AppendLine("HTTP/1.1 200 OK")
                .AppendLine("Content-Type: text/csv")
                .AppendLine("Content-Disposition: attachment; filename='TestHttpClient.csv'")
                .AppendLine("Content-Length: " + sampleCsvBytes.Length)
                .AppendLine()
                .AppendLine()
                .Append(SampleCsv);
            connectionSocket.Send(Encoding.UTF8.GetBytes(responseBuilder.ToString()));
        });

        var options = new CsvOptions();
        var factory = new CsvStreamFactory(options);

        var httpClient =
            new HttpClient
            {
                BaseAddress = new Uri($"http://{hostIP}:{port}/")
            };
        var httpDownloadMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("/TestHttpClient.csv", UriKind.Relative));
        var httpUploadMessage = new HttpRequestMessage(HttpMethod.Put, new Uri("/TestHttpClient.csv", UriKind.Relative));

        // Act
        listenThread.Start();
        using var csvStream = factory.From(httpClient, httpDownloadMessage, httpUploadMessage, TimeSpan.FromMinutes(1.0D));

        // Assert
        Assert.NotNull(csvStream);
        listeningSocket.Close();
    }

    [Fact(DisplayName = "CSV Stream Factory :: from Stream")]
    public void FromStreamTest()
    {
        // Arrange
        const string SampleCsv =
@"Abc,Def,Ghi,Jkl,Mno,Pqr,Stu,Vwx,Yz
1,2,3,4,5,6,7,8,nine";
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(SampleCsv));
        var options = new CsvOptions(HasHeaderLine: true);
        var factory = new CsvStreamFactory(options);

        // Act
        using var csvStream = factory.From(memoryStream);

        // Assert
        Assert.NotNull(csvStream);
        Assert.True(csvStream.Open);
        Assert.Equal(["Abc", "Def", "Ghi", "Jkl", "Mno", "Pqr", "Stu", "Vwx", "Yz"], csvStream.ColumnNames);
    }
}
