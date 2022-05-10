namespace Noodle_Assignment.Service
{
    public class CheckOutService : ICheckOutService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public CheckOutService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync(CheckoutModel model)
        {
            // TODO: GET customer

            // ex: 9215ed81-83a8-4741-b8cd-2a8f51dbce1a fistname: SSS7 lastname: PPP7 email: soma7@example.com

            var customer = await _client.WithApi().WithProjectKey(projectKey)
              .Customers()
              .WithId("9215ed81-83a8-4741-b8cd-2a8f51dbce1a")
              .Get()
              .ExecuteAsync();

            // TODO: CREATE a cart for the customer

            var myLineItemDraft = new LineItemDraft()
            {
                ProductId = "81237e3c-ef8e-4ba9-b035-78c0d6c10712",
                Sku= "M0E20000000F1YN",
                VariantId = 1,
                Quantity = 1
            };

            var lstLineItems = new List<ILineItemDraft>() { myLineItemDraft };

            var cartDraft = new CartDraft()
            {
                CustomerEmail = customer?.Email,
                BillingAddress = customer?.Addresses[0],
                LineItems = lstLineItems
            };
                        
            var cart = await _client.WithApi().WithProjectKey(projectKey)
                .Carts()
                .Post(cartDraft)
                .ExecuteAsync();

            Console.WriteLine($"Cart {cart.Id} for customer: {cart.CustomerId}");

            // TODO: GET a channel if your inventory mode will not be NONE

            // TODO: ADD items to the cart

            var actionForAddNewLineItem = new CartAddLineItemAction()
            {
                ProductId = "ad2cfc71-1f7b-47f7-8283-f2d12d6850e8",
                Sku = "M0E20000000E1PI",
                VariantId = 1,
                Quantity = 1
            };

            var cartUpdateWithNewLineItem = new CartUpdate()
            {
                Actions = new List<ICartUpdateAction>() { actionForAddNewLineItem },
                Version = cart?.Version ?? 0,
            };

            var updatedCartWithNewLineItem = await _client.WithApi().WithProjectKey(projectKey)
              .Carts()
              .WithId(cart?.Id)
              .Post(cartUpdateWithNewLineItem)
              .ExecuteAsync();

            // TODO: ADD discount coupon code to the cart
            string cartDiscountId = "e64f8af9-2bc4-41ee-864f-3d4b0d42365b";
            string cartDiscountKey = "SAVE-20";

            //soma todo for add discuson 

            // TODO: RECALCULATE the cart

            // TODO: ADD default shipping to the cart

            // TODO: CREATE a payment 

            // Console.WriteLine($"Payment Created with Id: {payment.Id}");

            // TODO: ADD transaction to the payment

            // TODO: ADD payment to the cart

            // TODO: CREATE order
            // Console.WriteLine($"Order Created with order number: {order.OrderNumber}");

            // TODO: UPDATE order state to Confirmed
            // Console.WriteLine($"Order state changed to: {order.OrderState.Value}");

            // TODO: GET custom workflow state for Order
            // TODO: UPDATE order custom workflow state

            // Console.WriteLine($"Order Workflow State changed to: {order.State?.Obj?.Name["en"]}");

            await Task.CompletedTask;
            return "";
        }
    }
}
