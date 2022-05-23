namespace Noodle_Assignment.Service
{
    public class CartMergingService : ICartMergingService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public CartMergingService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync(CartMergeModel cartMergeModel)
        {
            var channel = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Channels()
                .WithId(cartMergeModel.ChannelId) // e3e0d1d3-e0d4-432a-a9ad-fc2bb0eb21db
                .Get()
                .ExecuteAsync();

            var customer = await _client.WithApi()
                               .WithProjectKey(projectKey)
                               .Customers()
                               .WithId(cartMergeModel.CustomerId) //9215ed81-83a8-4741-b8cd-2a8f51dbce1a
                               .Get()
                               .ExecuteAsync();

            //Create Cart for customer
            var lineItemDraft = new LineItemDraft()
            {
                Sku = cartMergeModel.SKU, // "A0E2000000024BC",
                SupplyChannel = new ChannelResourceIdentifier { Id = channel.Id },

                Quantity = 1,
                ExternalPrice = Money.FromDecimal("INR", 399M),

            };

            var lineItemDrafts = new List<ILineItemDraft>() { lineItemDraft };
            var cartDraft = new CartDraft()
            {
                Currency = cartMergeModel.Currency, // INR
                CustomerId = customer.Id,
                CustomerEmail = customer.Email,
                LineItems = lineItemDrafts,
                BillingAddress = customer.Addresses[0]

            };

            var cart = await _client.WithApi()
                            .WithProjectKey(projectKey)
                            .Carts()
                            .Post(cartDraft)
                            .ExecuteAsync();

            //Create cart for annonymous

            var lineItemDraftForAnnonymous = new LineItemDraft()
            {
                Sku =cartMergeModel.SKU, // A0E2000000027DV
                SupplyChannel = new ChannelResourceIdentifier { Id = channel.Id },
                Quantity = 1,
                ExternalPrice = Money.FromDecimal(cartMergeModel.Currency, cartMergeModel.Price),

            };

            var lineItemDraftsForAnnonymous = new List<ILineItemDraft>() { lineItemDraftForAnnonymous };
            var annonymosCartDraft = new CartDraft()
            {
                Currency = cartMergeModel.Currency, // INR
                AnonymousId = "1234",

                Country = "DE",
                DeleteDaysAfterLastModification = 30,
                LineItems = lineItemDraftsForAnnonymous,
                BillingAddress = customer.Addresses[0]

            };

            var anonymousCart = await _client.WithApi()
                            .WithProjectKey(projectKey)
                            .Carts()
                            .Post(annonymosCartDraft)
                            .ExecuteAsync();

            var authentiCatedCustomer = new CustomerSignin()
            {

                AnonymousCart = new CartResourceIdentifier
                {
                    Id = anonymousCart.Id,

                },
                AnonymousId = anonymousCart.AnonymousId,
                AnonymousCartSignInMode = IAnonymousCartSignInMode.MergeWithExistingCustomerCart,
                Email = customer.Email,
                Password = "Test@123",
                UpdateProductData = true,
            };

            var result = await _client.WithApi().WithProjectKey(projectKey)
              .Login()
              .Post(authentiCatedCustomer)
              .ExecuteAsync();

            //LineItems of the anonymous cart will be copied to the customer’s active cart that has been modified most recently.
            var currentCustomerCart = result?.Cart as Cart;

            if (currentCustomerCart != null)
            {
                foreach (var lineItem in currentCustomerCart.LineItems)
                {
                    Console.WriteLine($"SKU: {lineItem.Variant.Sku}, Quantity: {lineItem.Quantity}");
                }
            }

            return "merge-cart completed";

        }
    }
}
