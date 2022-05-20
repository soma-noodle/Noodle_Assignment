namespace Noodle_Assignment.Service
{
    public class InStoreService : IInStoreService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public InStoreService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            var customer = await _client.WithApi()
                                .WithProjectKey(projectKey)
                                .Customers()
                                .WithId("9215ed81-83a8-4741-b8cd-2a8f51dbce1a")
                                .Get()
                                .ExecuteAsync();

            var store = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Stores()
                .WithId("4b92ce2d-9f02-4e11-90fb-d366e41813a2") //DACH region
                .Get()
                .ExecuteAsync();

            var lineItemDraft = new LineItemDraft()
            {

                ProductId = "661a95b5-fe1a-4a53-bd0e-f6de70951c50",
                VariantId = 1,
                Quantity = 1,
                ExternalPrice = Money.FromDecimal("INR", 199M)

            };
            var lineItemDrafts = new List<ILineItemDraft>() { lineItemDraft };
            var storeResource = new StoreResourceIdentifier()
            {
                Id = store.Id
            };
            var cartDraft = new CartDraft()
            {
                Currency = "INR",
                CustomerId = customer.Id,
                CustomerEmail = customer.Email,
                LineItems = lineItemDrafts,
                BillingAddress = customer.Addresses[0],
                Store = storeResource

            };
            var storeCart = await _client.WithApi()
                .WithProjectKey(projectKey)

                .InStoreKeyWithStoreKeyValue(store.Key)
                .Carts()
                .Post(cartDraft)
                .ExecuteAsync();

            return $"Cart {storeCart.Id} created in store {store.Key} for customer {customer.FirstName}";
        }
    }
}
