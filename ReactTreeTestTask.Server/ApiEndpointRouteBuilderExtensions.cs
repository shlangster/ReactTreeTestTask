using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ReactTreeTestTask.Server.Data;
using ReactTreeTestTask.Server.Interfaces;

namespace ReactTreeTestTask.Server
{
    public static class ApiEndpointRouteBuilderExtensions
    {
        public static void MapApiEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            endpoints.MapPost("/api.user.tree.get", async Task<Results<Ok<NodeDto>, ProblemHttpResult>>
                ([FromQuery] string treeName, [FromServices] IServiceProvider sp) =>
            {
                if(treeName == null)
                    throw new SecureException("The treeName field is required.");

                var treeService = sp.GetRequiredService<ITreeService>();
                return TypedResults.Ok<NodeDto>(await treeService.GetTree(treeName));
            }).WithDescription("Returns your entire tree. If your tree doesn't exist it will be created automatically.")
            .WithTags("user.tree")
            .WithOpenApi();
            

            endpoints.MapPost("/api.user.tree.node.create", async Task<Results<Ok, ProblemHttpResult>>
                ([FromQuery] string treeName, [FromQuery] int parentNodeId, [FromQuery] string nodeName, [FromServices] IServiceProvider sp) =>
            {
                if (treeName == null)
                    throw new SecureException("The treeName field is required.");

                if (parentNodeId == 0)
                    throw new SecureException("The parentNodeId field is required.");

                if (nodeName == null)
                    throw new SecureException("The nodeName field is required.");

                var treeService = sp.GetRequiredService<ITreeService>();

                await treeService.CreateNode(treeName, parentNodeId, nodeName);
                return TypedResults.Ok();
            }).WithDescription("Create a new node in your tree. You must to specify a parent node ID that belongs to your tree. A new node name must be unique across all siblings.")
            .WithTags("user.tree.node")
            .WithOpenApi();

            endpoints.MapPost("/api.user.tree.node.delete", async Task<Results<Ok, ProblemHttpResult>>
                ([FromQuery] string treeName, [FromQuery] int nodeId, [FromServices] IServiceProvider sp) =>
            {
                if (treeName == null)
                    throw new SecureException("The treeName field is required.");

                if (nodeId == 0)
                    throw new SecureException("The nodeId field is required.");

                var treeService = sp.GetRequiredService<ITreeService>();

                await treeService.DeleteNode(treeName, nodeId);
                return TypedResults.Ok();
            }).WithDescription("Delete an existing node in your tree. You must specify a node ID that belongs your tree.")
            .WithTags("user.tree.node")
            .WithOpenApi();

            endpoints.MapPost("/api.user.tree.node.rename", async Task<Results<Ok, ProblemHttpResult>>
                ([FromQuery] string treeName, [FromQuery] int nodeId, [FromQuery] string newNodeName, [FromServices] IServiceProvider sp) =>
            {
                if (treeName == null)
                    throw new SecureException("The treeName field is required.");

                if (nodeId == 0)
                    throw new SecureException("The nodeId field is required.");

                if (newNodeName == null)
                    throw new SecureException("The newNodeName field is required.");

                var treeService = sp.GetRequiredService<ITreeService>();

                await treeService.RenameNode(treeName, nodeId, newNodeName);
                return TypedResults.Ok();
            }).WithDescription("Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.")
            .WithTags("user.tree.node")
            .WithOpenApi();

            endpoints.MapPost("/api.user.journal.getRange", async Task<Results<Ok<JournalRangeDto>, ProblemHttpResult>>
                ([FromQuery] int skip, [FromQuery] int take, [FromBody] JournalFilter filter,[FromServices] IServiceProvider sp) =>
            {
                var journalService = sp.GetRequiredService<IJournalService>();

                return TypedResults.Ok(await journalService.GetRange(skip, take, filter));
            }).WithDescription("Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.")
            .WithTags("user.journal")
            .WithOpenApi();

            endpoints.MapPost("/api.user.journal.getSingle", async Task<Results<Ok<JournalDto>, ProblemHttpResult>>
                ([FromQuery] int id, [FromServices] IServiceProvider sp) =>
            {
                if (id == 0)
                    throw new SecureException("The id field is required.");
                var journalService = sp.GetRequiredService<IJournalService>();

                return TypedResults.Ok(await journalService.GetSingle(id));
            }).WithDescription("Returns the information about an particular event by ID.")
            .WithTags("user.journal")
            .WithOpenApi();

        }

    }
}
