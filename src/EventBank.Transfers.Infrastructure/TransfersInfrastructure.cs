using EventBank.BuildingBlocks.EventSourcing;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Transfers.Application.Accounts;
using EventBank.Transfers.Application.Queries;
using EventBank.Transfers.Domain;
using EventBank.Transfers.Infrastructure.Integration;
using EventBank.Transfers.Infrastructure.Persistence;
using EventBank.Transfers.Infrastructure.Projections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBank.Transfers.Infrastructure;

public static class TransfersInfrastructure
{
    public static IServiceCollection AddTransfersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Transfers")
            ?? throw new InvalidOperationException("Connection string 'Transfers' is missing.");

        services.AddEventSourcing<TransfersDbContext>(connectionString, typeof(Transfer).Assembly);
        services.AddAggregateRepository<Transfer, TransferId>();
        services.AddProjection<TransfersDbContext, TransferReadModelProjection>();
        services.AddProjectionSubscription<TransfersDbContext>();
        services.AddIntegrationEventPublishing<TransfersDbContext, TransfersIntegrationEventMapper>();
        services.AddScoped<IAccountDirectory, EfAccountDirectory>();
        services.AddScoped<ITransferReadStore, EfTransferReadStore>();
        services.AddRabbitMq(configuration);

        return services;
    }
}
