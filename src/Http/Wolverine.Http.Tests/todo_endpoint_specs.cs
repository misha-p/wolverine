using WolverineWebApi.Samples;

namespace Wolverine.Http.Tests;

/// <summary>
/// These specs were mostly built to support the HTTP enhancements in Wolverine 1.2
/// </summary>
public class todo_endpoint_specs : IntegrationContext
{
    public todo_endpoint_specs(AppFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task wolverine_can_handle_route_constraints()
    {
        await using var session = Store.LightweightSession();
        var todo = new Todo { Name = "First", IsComplete = false };
        session.Store(todo);
        await session.SaveChangesAsync();

        await Scenario(opts =>
        {
            opts.Put.Json(new UpdateRequest("Second", true)).ToUrl("/todos/" + todo.Id);
            opts.StatusCodeShouldBe(204);
        });
    }
}