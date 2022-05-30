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

        public async Task<string> ExecuteAsync(InStoreModel inStoreModel)
        {
            var customer = await _client.WithApi()
                                .WithProjectKey(projectKey)
                                .Customers()
                                .WithId(inStoreModel.CustomerId)
                                .Get()
                                .ExecuteAsync();

            var store = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Stores()
                .WithId(inStoreModel.StoreId)
                .Get()
                .ExecuteAsync();

            var lineItemDraft = new LineItemDraft()
            {
                ProductId = inStoreModel.LineItemModel.ProductId,
                VariantId = inStoreModel.LineItemModel.VariantId,

                ExternalPrice = Money.FromDecimal(inStoreModel.LineItemModel.Currency, inStoreModel.LineItemModel.Price)
            };

            var lineItemDrafts = new List<ILineItemDraft>() { lineItemDraft };

            var storeResource = new StoreResourceIdentifier()
            {
                Id = store.Id
            };

            var cartDraft = new CartDraft()
            {
                Currency = inStoreModel.LineItemModel.Currency,
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
