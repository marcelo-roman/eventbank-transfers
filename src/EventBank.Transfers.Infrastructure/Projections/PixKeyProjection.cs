using EventBank.BuildingBlocks.EventSourcing.Projections;
using EventBank.BuildingBlocks.EventSourcing.Store;
using EventBank.Transfers.Domain.Pix;
using EventBank.Transfers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventBank.Transfers.Infrastructure.Projections;

public sealed class PixKeyProjection : IProjection<TransfersDbContext>
{
    public async Task Project(EventEnvelope envelope, TransfersDbContext dbContext, CancellationToken cancellationToken)
    {
        switch (envelope.Event)
        {
            case PixKeyRegistered registered:
                var key = await dbContext.PixKeys.FindAsync([registered.Key], cancellationToken);
                if (key is null)
                {
                    key = new PixKeySummary { Key = registered.Key, Type = registered.Type, OwnerName = registered.OwnerName };
                    dbContext.PixKeys.Add(key);
                }

                key.Type = registered.Type;
                key.AccountId = registered.AccountId;
                key.OwnerId = registered.OwnerId;
                key.OwnerName = registered.OwnerName;
                key.IsActive = true;
                key.RegisteredAt = envelope.RecordedAt;
                break;
            case PixKeyRemoved removed:
                var existing = await dbContext.PixKeys.FindAsync([removed.Key], cancellationToken);
                existing?.IsActive = false;
                break;
        }
    }

    public Task Reset(TransfersDbContext dbContext, CancellationToken cancellationToken) =>
        dbContext.PixKeys.ExecuteDeleteAsync(cancellationToken);
}
