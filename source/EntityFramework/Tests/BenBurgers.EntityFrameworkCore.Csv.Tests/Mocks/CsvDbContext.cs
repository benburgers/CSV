/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Builders;
using BenBurgers.EntityFrameworkCore.Csv.Storage;
using Microsoft.EntityFrameworkCore;

namespace BenBurgers.EntityFrameworkCore.Csv.Tests.Mocks;

public sealed class CsvDbContext : DbContext
{
    public CsvDbContext(DbContextOptions<CsvDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var mockRecord = modelBuilder.Entity<MockRecord>();
        mockRecord.HasKey(mr => mr.MockInt);
        mockRecord.Property(mr => mr.MockString);
        mockRecord.ToCsvDataSource(new CsvFileSource("MockRecordTest.csv", hasColumnNamesRow: true));
        mockRecord.HasData(
            new MockRecord(1, "Test 1"),
            new MockRecord(2, "Test 2"));
        base.OnModelCreating(modelBuilder);
    }
}
