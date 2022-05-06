namespace Noodle_Assignment.Service
{
    public class ProductSelectionsService : IProductSelectionsService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public ProductSelectionsService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            await Task.CompletedTask;
            return "";
        }
    }
}
