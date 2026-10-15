using System.Reflection;

namespace EventBank.Transfers.Architecture.Tests;

internal static class Layers
{
    public const string Root = "EventBank.Transfers";

    public const string OtherBoundedContext = "EventBank.Accounts";

    public static readonly Assembly Domain = typeof(Domain.Transfer).Assembly;

    public static readonly Assembly Application = typeof(Application.TransfersApplication).Assembly;

    public static readonly Assembly Infrastructure = typeof(Infrastructure.TransfersInfrastructure).Assembly;

    public static readonly Assembly Api = typeof(Api.Endpoints.TransferEndpoints).Assembly;

    public static readonly Assembly[] All = [Domain, Application, Infrastructure, Api];
}
