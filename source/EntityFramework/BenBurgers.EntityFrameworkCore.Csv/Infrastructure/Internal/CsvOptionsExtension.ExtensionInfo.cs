/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Globalization;
using System.Text;

namespace BenBurgers.EntityFrameworkCore.Csv.Infrastructure.Internal;

internal partial class CsvOptionsExtension
{
    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        private string? logFragment;
        private int? serviceProviderHash;

        /// <summary>
        /// Initializes a new instance of <see cref="ExtensionInfo" />.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public ExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        /// <inheritdoc />
        public override CsvOptionsExtension Extension => (CsvOptionsExtension)base.Extension;

        /// <inheritdoc />
        public override bool IsDatabaseProvider => true;

        /// <inheritdoc />
        public override string LogFragment
        {
            get
            {
                if (this.logFragment is null)
                {
                    var builder = new StringBuilder();
                    if (this.Extension.DirectoryDefault is { FullName: { } defaultDirectoryFullName })
                        builder.Append(nameof(DirectoryDefault)).Append('=').Append(defaultDirectoryFullName).Append(' ');
                    this.logFragment = builder.ToString();
                }
                return this.logFragment;
            }
        }

        /// <inheritdoc />
        public override int GetServiceProviderHashCode()
        {
            if (this.serviceProviderHash is null)
            {
                var hashCode = new HashCode();
                hashCode.Add(this.Extension.DirectoryDefault?.GetHashCode());
                this.serviceProviderHash = hashCode.ToHashCode();
            }
            return this.serviceProviderHash.Value;
        }

        /// <inheritdoc />
        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            if (this.Extension.DirectoryDefault is { FullName: { } defaultDirectoryFullName })
                debugInfo[$"{CsvAnnotationNames.Prefix}:{nameof(DirectoryDefault)}"] =
                    defaultDirectoryFullName.GetHashCode().ToString(CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is ExtensionInfo info
                && this.Extension.DirectoryDefault?.FullName == info.Extension.DirectoryDefault?.FullName;
        }
    }
}
