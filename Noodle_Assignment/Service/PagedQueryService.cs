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

            IProductPagedQueryResponse? response = null;
            int totCount = 0;

            while (!lastPage)
            {
                var where = lastId != null ? $"id>\"{lastId}\"" : null;
                
                // TODO: GET paged response sorted on id

                if (where == null)
                {

                    response = await _client.WithApi()
                       .WithProjectKey(projectKey)
                       .Products()
                       .Get()
                       .WithSort("id")
                       .WithLimit(500)
                       .ExecuteAsync();
                }
                else
                {
                    response = await _client.WithApi()
                   .WithProjectKey(projectKey)
                   .Products()
                   .Get()
                   .WithSort("id")
                   .WithWhere(where)
                   .WithLimit(500)
                   .ExecuteAsync();
                }

                if (response.Results.Any())
                {
                    //Console.WriteLine($"Show Results of Page {currentPage}");
                    //foreach (var product in response.Results)
                    //{
                    //    if (product.MasterData.Current.Name.ContainsKey("en"))
                    //    {
                    //        Console.WriteLine($"{product.MasterData.Current.Name["en"]}");
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine($"{product.MasterData.Current.Name["de"]}");
                    //    }
                    //}
                    //Console.WriteLine("///////////////////////");

                    currentPage++;
                    lastId = response.Results.Last().Id;
                    totCount += response.Results.Count;
                }

                lastPage = response?.Results.Count < pageSize;               
            }

            return "Total Count: " + totCount;
        }
    }
}
