using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.Transfers.Domain.Pix;

namespace EventBank.Transfers.Application.Pix;

public sealed class LookupPixKeyHandler(IPixKeyReadStore readStore) : IQueryHandler<LookupPixKey, PixKeyLookupView>
{
    public async Task<PixKeyLookupView> Handle(LookupPixKey query, CancellationToken cancellationToken)
    {
        var key = PixKeyValue.Detect(query.Key);
        var active = await readStore.FindActive(key.Value, cancellationToken) ?? throw PixErrors.KeyNotRegistered();
        return new PixKeyLookupView(key.Masked(), active.Type, PixKeyPresentation.ShortName(active.OwnerName), PixKeyPresentation.Institution);
    }
}
