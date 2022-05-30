namespace Noodle_Assignment.Service
{
    public class CustomObjectsService : ICustomObjectsService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public CustomObjectsService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            StreamReader r = new StreamReader("Resources/compatibility-info.json");
            string jsonString = r.ReadToEnd();
            var m = JsonConvert.DeserializeObject(jsonString);
            var customObjectDraft = new CustomObjectDraft()
            {
                Key = "custom_object",
                Container = "myContainer",
                Value = m
            };

            var customObject = await _client.WithApi()
                .WithProjectKey(projectKey)
                .CustomObjects()
                .Post(customObjectDraft)
                .ExecuteAsync();


            return $"custom object created with Id {customObject.Id} with version {customObject.Version}";
        }
    }
}
