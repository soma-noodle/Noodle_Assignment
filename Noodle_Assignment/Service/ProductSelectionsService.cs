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
            //Create Product Selection
            var productSelectionDraft = new ProductSelectionDraft()
            {
                Key = Guid.NewGuid().ToString(),
                Name = new LocalizedString { { "product", "product" } }
            };
            
            var productSelection = await _client.WithApi()
                      .WithProjectKey(projectKey)
                      .ProductSelections()
                      .Post(productSelectionDraft)
                      .ExecuteAsync();
            
            //Add product to the selection
            var addedProduct = new ProductSelectionAddProductAction()
            {
                Product = new ProductResourceIdentifier() { Id = "e75d6e69-ed6e-4aea-ab59-e2641f2f927e" }
            };

            var updateProductSelection = new ProductSelectionUpdate()
            {
                Actions = new List<IProductSelectionUpdateAction> { addedProduct },
                Version = productSelection?.Version ?? 0
            };

            var updatedProductSelection = await _client.WithApi()
                .WithProjectKey(projectKey)
                .ProductSelections()
                .WithId(productSelection?.Id)
                .Post(updateProductSelection)
                .ExecuteAsync();

            var store = await _client.WithApi()
               .WithProjectKey(projectKey)
               .Stores()
               .WithId("4b92ce2d-9f02-4e11-90fb-d366e41813a2")
               .Get()
               .ExecuteAsync();
            
            var addProductSelection = new StoreAddProductSelectionAction()
            {
                ProductSelection = new ProductSelectionResourceIdentifier() { Id = productSelection?.Id },
                Active = true,
            };

            var storeUpdate = new StoreUpdate()
            {
                Actions = new List<IStoreUpdateAction> { addProductSelection },
                Version = store.Version
            };

            var updatedStore = await _client.WithApi()
                 .WithProjectKey(projectKey)
                 .Stores()
                 .WithId("4b92ce2d-9f02-4e11-90fb-d366e41813a2")
                 .Post(storeUpdate)
                 .ExecuteAsync();
            
            return $"Updated store {updatedStore.Key} with selection {updatedStore.ProductSelections?.Count}";
        }
    }
}
