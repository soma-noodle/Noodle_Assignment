namespace Noodle_Assignment.Service
{
    public class ApiExtensionService : IApiExtensionService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public ApiExtensionService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            var extensionTrigger = new ExtensionTrigger()
            {
                Actions = new List<IExtensionAction>() { IExtensionAction.Create },
                ResourceTypeId = IExtensionResourceTypeId.Order
            };

            var httpDestination = new HttpDestination()
            {
                Type = "HTTP",
                Url = "https://api.australia-southeast1.gcp.commercetools.com/"
            };


            var extensionDraft = new ExtensionDraft()
            {
                Destination = httpDestination,
                Triggers = new List<IExtensionTrigger>() { extensionTrigger },
                Key = "Demo"
            };

            var extension = await _client.WithApi()
             .WithProjectKey(projectKey)
             .Extensions()
             .Post(extensionDraft)
             .ExecuteAsync();

            return $"extension created with Id {extension.Id}";
        }
    }
}
