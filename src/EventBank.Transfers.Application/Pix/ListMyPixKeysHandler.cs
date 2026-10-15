using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Security;

namespace EventBank.Transfers.Application.Pix;

public sealed class ListMyPixKeysHandler(IPixKeyReadStore readStore, ICurrentUser currentUser)
    : IQueryHandler<ListMyPixKeys, IReadOnlyList<PixKeyView>>
{
    public async Task<IReadOnlyList<PixKeyView>> Handle(ListMyPixKeys query, CancellationToken cancellationToken)
    {
        var keys = await readStore.ListActiveOf(currentUser.Id, cancellationToken);
        return [.. keys.Select(key => new PixKeyView(key.Key, key.Type, PixKeyPresentation.Mask(key), key.AccountId, key.RegisteredAt))];
    }
}
