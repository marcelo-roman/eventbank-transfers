using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Security;
using EventBank.Transfers.Application.Pix;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventBank.Transfers.Api.Endpoints;

public static class PixEndpoints
{
    public static IEndpointRouteBuilder MapPixEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var pix = endpoints.MapGroup("/pix").WithTags("PIX").RequireAuthorization(Roles.Customer);

        pix.MapGet("/keys", ListKeys);
        pix.MapPost("/keys", RegisterKey);
        pix.MapDelete("/keys/{key}", RemoveKey);
        pix.MapGet("/keys/lookup", Lookup);
        pix.MapPost("/transfers", Send);

        return endpoints;
    }

    private static async Task<Ok<IReadOnlyList<PixKeyView>>> ListKeys(
        IQueryHandler<ListMyPixKeys, IReadOnlyList<PixKeyView>> handler,
        CancellationToken cancellationToken) =>
        TypedResults.Ok(await handler.Handle(new ListMyPixKeys(), cancellationToken));

    private static async Task<Created<CommandResult>> RegisterKey(
        RegisterPixKeyRequest request,
        ICommandHandler<RegisterPixKey, CommandResult> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new RegisterPixKey(request.AccountId, request.Type, request.Value), cancellationToken);
        return TypedResults.Created("/pix/keys", result);
    }

    private static async Task<Ok<CommandResult>> RemoveKey(
        string key,
        ICommandHandler<RemovePixKey, CommandResult> handler,
        CancellationToken cancellationToken) =>
        TypedResults.Ok(await handler.Handle(new RemovePixKey(key), cancellationToken));

    private static async Task<Ok<PixKeyLookupView>> Lookup(
        string key,
        IQueryHandler<LookupPixKey, PixKeyLookupView> handler,
        CancellationToken cancellationToken) =>
        TypedResults.Ok(await handler.Handle(new LookupPixKey(key), cancellationToken));

    private static async Task<Accepted<CommandResult>> Send(
        SendPixRequest request,
        ICommandHandler<SendPix, CommandResult> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new SendPix(request.SourceAccountId, request.Key, request.Amount), cancellationToken);
        return TypedResults.Accepted($"/transfers/{result.Id}", result);
    }
}
