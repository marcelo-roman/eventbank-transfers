using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.BuildingBlocks.Web;
using EventBank.Transfers.Application.Commands;
using EventBank.Transfers.Application.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventBank.Transfers.Api.Endpoints;

public static class TransferEndpoints
{
    public static IEndpointRouteBuilder MapTransferEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var transfers = endpoints.MapGroup("/transfers").WithTags("Transfers");

        transfers.MapPost("/", Request);
        transfers.MapGet("/", List);
        transfers.MapGet("/{transferId:guid}", Get);
        transfers.MapGet("/{transferId:guid}/events", Events);

        return endpoints;
    }

    private static async Task<Accepted<CommandResult>> Request(
        RequestTransferRequest request,
        ICommandHandler<RequestTransfer, CommandResult> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new RequestTransfer(request.SourceAccountId, request.DestinationAccountId, request.Amount),
            cancellationToken);
        return TypedResults.Accepted($"/transfers/{result.Id}", result);
    }

    private static async Task<Ok<IReadOnlyList<TransferView>>> List(
        Guid? accountId,
        IQueryHandler<ListTransfers, IReadOnlyList<TransferView>> handler,
        CancellationToken cancellationToken) =>
        TypedResults.Ok(await handler.Handle(new ListTransfers(accountId), cancellationToken));

    private static async Task<Results<Ok<TransferView>, NotFound>> Get(
        Guid transferId,
        IQueryHandler<GetTransfer, TransferView?> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new GetTransfer(transferId), cancellationToken) is { } transfer
            ? TypedResults.Ok(transfer)
            : TypedResults.NotFound();

    private static async Task<Ok<IReadOnlyList<StoredEventResponse>>> Events(
        Guid transferId,
        IQueryHandler<GetTransferEvents, IReadOnlyList<RecordedEvent>> handler,
        CancellationToken cancellationToken) =>
        TypedResults.Ok(StoredEventResponse.From(await handler.Handle(new GetTransferEvents(transferId), cancellationToken)));
}
