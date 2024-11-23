/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Tests.Bootstrapping;
using BenBurgers.Text.Csv.Tests.Resources;
using System.Text;

namespace BenBurgers.Text.Csv.Tests.CsvStreams;

[Collection(nameof(ConfigurationTestCollection))]
public partial class CsvStreamTests
{
    private readonly ConfigurationTestFixture configurationTestFixture;

    public CsvStreamTests(ConfigurationTestFixture configurationTestFixture)
    {
        this.configurationTestFixture = configurationTestFixture;
    }

    private static Stream GetSample(string name)
    {
        var sampleString = CsvSamples.ResourceManager.GetString(name)!;
        return new MemoryStream(Encoding.UTF8.GetBytes(sampleString));
    }
}
