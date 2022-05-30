namespace Noodle_Assignment.Service
{
    public class SearchService : ISearchService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public SearchService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            var productType = await _client.WithApi()
                .WithProjectKey(projectKey)
                .ProductTypes()
                .WithKey("main")
                .Get()
                .ExecuteAsync();

            var filterQuery = $"productType.id:\"{productType.Id}\"";

            var facet = "variants.attributes.color as color";

            var formParams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("filter", filterQuery), new KeyValuePair<string, string>("facet", facet) };

            var searchResponse = await _client.WithApi()
                .WithProjectKey(projectKey)
                .ProductProjections()
                .Search()
                .Post(formParams)
                .ExecuteAsync();

            Console.WriteLine($"No. of products: {searchResponse.Count}");

            Console.WriteLine("products in search result: ");

            searchResponse.Results.ForEach(p => Console.WriteLine(p.Name["en"]));

            //Console.WriteLine($"Number of Facets: {productProjection.Facets.Count}");

            var colorFacetResult = searchResponse.Facets["color"] as TermFacetResult;

            foreach (var term in colorFacetResult?.Terms)
            {
                Console.WriteLine($"Term : {term.Term}, Count: {term.Count}");
            }

            return "Search completed successfully";
        }
    }
}
