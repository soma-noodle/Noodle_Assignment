namespace Noodle_Assignment.Service
{
    public class GraphQLService : IGraphQLService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public GraphQLService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<List<string>> ExecuteAsync()
        {
            var graphRequest = new GraphQLRequest
            {
                Query = "query {customers{count,results{email}}}"
            };

            // TODO: graphQL Request
            var response = await _client.WithApi().WithProjectKey(projectKey)
              .Graphql()
              .Post(graphRequest)
              .ExecuteAsync();

            //Map Response to the typed result and show it
            var typedResult = ((JsonElement)response.Data).ToObject<GraphResultData>(_client.SerializerService);
            var customersResult = typedResult.Customers;

            return customersResult.Results.Select(x => x.Email).ToList();
        }
    }
}
