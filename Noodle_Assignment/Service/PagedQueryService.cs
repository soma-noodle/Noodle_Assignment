namespace Noodle_Assignment.Service
{
    public class PagedQueryService : IPagedQueryService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public PagedQueryService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            string lastId = null; int pageSize = 2; int currentPage = 1; bool lastPage = false;
            IProductPagedQueryResponse response;

            //response =  await _client.WithApi().WithProjectKey(projectKey)
            //   .Products()
            //   .get()
            //   .ExecuteAsync();
            while (!lastPage)
            {
                var where = lastId != null ? $"id>\"{lastId}\"" : null;
                // TODO: GET paged response sorted on id
                response = null;

                Console.WriteLine($"Show Results of Page {currentPage}");
                foreach (var product in response.Results)
                {
                    if (product.MasterData.Current.Name.ContainsKey("en"))
                    {
                        Console.WriteLine($"{product.MasterData.Current.Name["en"]}");
                    }
                    else
                    {
                        Console.WriteLine($"{product.MasterData.Current.Name["de"]}");
                    }
                }
                Console.WriteLine("///////////////////////");
                currentPage++;
                lastId = response.Results.Last().Id;
                lastPage = response.Results.Count < pageSize;
            }

            await Task.CompletedTask;
            return "";
        }
    }
}
