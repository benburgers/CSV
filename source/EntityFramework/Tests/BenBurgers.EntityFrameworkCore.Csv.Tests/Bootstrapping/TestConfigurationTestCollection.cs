/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Bootstrapping;

[CollectionDefinition(nameof(TestConfigurationTestCollection))]
public sealed class TestConfigurationTestCollection : ICollectionFixture<TestConfigurationFixture>
{
}
