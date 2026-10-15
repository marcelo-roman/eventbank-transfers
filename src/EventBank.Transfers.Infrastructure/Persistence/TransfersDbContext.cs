using EventBank.BuildingBlocks.EventSourcing.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class TransfersDbContext(DbContextOptions<TransfersDbContext> options) : DbContext(options)
{
    public DbSet<TransferSummary> TransferSummaries => Set<TransferSummary>();

    public DbSet<AccountDirectoryEntry> AccountDirectory => Set<AccountDirectoryEntry>();

    public DbSet<CustomerDirectoryEntry> CustomerDirectory => Set<CustomerDirectoryEntry>();

    public DbSet<PixKeySummary> PixKeys => Set<PixKeySummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddSubscriptionCheckpoints();

        modelBuilder.Entity<TransferSummary>(summary =>
        {
            summary.ToTable("transfer_summaries");
            summary.HasKey(entry => entry.Id);
            summary.HasIndex(entry => entry.SourceAccountId);
            summary.HasIndex(entry => entry.DestinationAccountId);
            summary.Property(entry => entry.SourceHolder).HasMaxLength(120);
            summary.Property(entry => entry.DestinationHolder).HasMaxLength(120);
            summary.Property(entry => entry.Currency).HasMaxLength(3);
            summary.Property(entry => entry.Status).HasMaxLength(20);
            summary.Property(entry => entry.FailureReason).HasMaxLength(100);
            summary.Property(entry => entry.Amount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<AccountDirectoryEntry>(account =>
        {
            account.ToTable("account_directory");
            account.HasKey(entry => entry.AccountId);
            account.HasIndex(entry => entry.OwnerId);
            account.Property(entry => entry.Holder).HasMaxLength(120);
            account.Property(entry => entry.Currency).HasMaxLength(3);
            account.Property(entry => entry.Number).HasMaxLength(13);
        });

        modelBuilder.Entity<CustomerDirectoryEntry>(customer =>
        {
            customer.ToTable("customer_directory");
            customer.HasKey(entry => entry.CustomerId);
            customer.Property(entry => entry.FullName).HasMaxLength(120);
            customer.Property(entry => entry.Email).HasMaxLength(200);
            customer.Property(entry => entry.Cpf).HasMaxLength(11);
        });

        modelBuilder.Entity<PixKeySummary>(key =>
        {
            key.ToTable("pix_keys");
            key.HasKey(entry => entry.Key);
            key.HasIndex(entry => entry.OwnerId);
            key.HasIndex(entry => entry.AccountId);
            key.Property(entry => entry.Key).HasMaxLength(80);
            key.Property(entry => entry.Type).HasMaxLength(10);
            key.Property(entry => entry.OwnerName).HasMaxLength(120);
        });
    }
}
