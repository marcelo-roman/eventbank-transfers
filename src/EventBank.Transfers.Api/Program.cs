using EventBank.BuildingBlocks.Messaging;
using EventBank.BuildingBlocks.Web;
using EventBank.Contracts;
using EventBank.Transfers.Api.Consumers;
using EventBank.Transfers.Api.Endpoints;
using EventBank.Transfers.Application;
using EventBank.Transfers.Infrastructure;
using EventBank.Transfers.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services
    .AddTransfersApplication()
    .AddTransfersInfrastructure(builder.Configuration)
    .AddMessageHandler<AccountOpened, AccountOpenedConsumer>()
    .AddMessageHandler<AccountClosed, AccountClosedConsumer>()
    .AddMessageHandler<AccountDebited, AccountDebitedConsumer>()
    .AddMessageHandler<AccountDebitDeclined, AccountDebitDeclinedConsumer>()
    .AddMessageHandler<AccountCredited, AccountCreditedConsumer>()
    .AddMessageHandler<AccountCreditDeclined, AccountCreditDeclinedConsumer>()
    .AddMessageHandler<AccountRefunded, AccountRefundedConsumer>();

var app = builder.Build();

app.UseServiceDefaults();
app.MapTransferEndpoints();
app.MapOperationsEndpoints<TransfersDbContext>();

await app.InitializeDatabase<TransfersDbContext>();
await app.RunAsync();
