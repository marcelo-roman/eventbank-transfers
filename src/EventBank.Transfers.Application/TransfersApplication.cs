using EventBank.BuildingBlocks.Application.Cqrs;
using Microsoft.Extensions.DependencyInjection;

namespace EventBank.Transfers.Application;

public static class TransfersApplication
{
    public static IServiceCollection AddTransfersApplication(this IServiceCollection services) =>
        services.AddCqrsHandlers(typeof(TransfersApplication).Assembly);
}
