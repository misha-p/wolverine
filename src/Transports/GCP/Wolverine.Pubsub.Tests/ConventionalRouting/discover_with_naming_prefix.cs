using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Wolverine.Runtime;
using Xunit;
using Xunit.Abstractions;

namespace Wolverine.Pubsub.Tests.ConventionalRouting;

public class discover_with_naming_prefix : IDisposable
{
    private readonly IHost _host;
    private readonly ITestOutputHelper _output;

    public discover_with_naming_prefix(ITestOutputHelper output)
    {
        _output = output;
        _host = Host
            .CreateDefaultBuilder()
            .UseWolverine(opts =>
            {
                opts
                    .UsePubsubTesting()
                    .AutoProvision()
                    .AutoPurgeOnStartup()
                    .EnableDeadLettering()
                    .EnableSystemEndpoints()
                    .PrefixIdentifiers("zztop")
                    .UseConventionalRouting();
            }).Start();
    }

    public void Dispose()
    {
        _host.Dispose();
    }

    [Fact]
    public void discover_listener_with_prefix()
    {
        var runtime = _host.Services.GetRequiredService<IWolverineRuntime>();
        var uris = runtime.Endpoints.ActiveListeners().Select(x => x.Uri).ToArray();

        uris.ShouldContain(new Uri($"{PubsubTransport.ProtocolName}://wolverine/zztop.routed"));
        uris.ShouldContain(
            new Uri($"{PubsubTransport.ProtocolName}://wolverine/zztop.Wolverine.Pubsub.Tests.TestPubsubMessage"));
    }
}