using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.Transfers.Application.Commands;
using EventBank.Transfers.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace EventBank.Transfers.Application;

public static class TransfersApplication
{
    public static IServiceCollection AddTransfersApplication(this IServiceCollection services) =>
        services
            .AddCqrsHandlers(typeof(TransfersApplication).Assembly)
            .AddScoped<TransferInitiation>()
            .AddScoped<TransferAccess>();
}
