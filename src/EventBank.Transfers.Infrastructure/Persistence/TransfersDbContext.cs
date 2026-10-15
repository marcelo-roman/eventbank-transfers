using EventBank.BuildingBlocks.EventSourcing.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class TransfersDbContext(DbContextOptions<TransfersDbContext> options) : DbContext(options)
{
    public DbSet<TransferSummary> TransferSummaries => Set<TransferSummary>();

    public DbSet<AccountDirectoryEntry> AccountDirectory => Set<AccountDirectoryEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddSubscriptionCheckpoints();

        modelBuilder.Entity<TransferSummary>(summary =>
        {
            summary.ToTable("transfer_summaries");
            summary.HasKey(entry => entry.Id);
            summary.HasIndex(entry => entry.SourceAccountId);
            summary.HasIndex(entry => entry.DestinationAccountId);
            summary.Property(entry => entry.SourceHolder).HasMaxLength(100);
            summary.Property(entry => entry.DestinationHolder).HasMaxLength(100);
            summary.Property(entry => entry.Currency).HasMaxLength(3);
            summary.Property(entry => entry.Status).HasMaxLength(20);
            summary.Property(entry => entry.FailureReason).HasMaxLength(100);
            summary.Property(entry => entry.Amount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<AccountDirectoryEntry>(account =>
        {
            account.ToTable("account_directory");
            account.HasKey(entry => entry.AccountId);
            account.Property(entry => entry.Holder).HasMaxLength(100);
            account.Property(entry => entry.Currency).HasMaxLength(3);
        });
    }
}
