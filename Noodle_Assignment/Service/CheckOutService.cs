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
              .WithId(model.CustomerId)
              .Get()
              .ExecuteAsync();

            // TODO: CREATE a cart for the customer

            var myLineItemDraft = new LineItemDraft()
            {
                ProductId = "81237e3c-ef8e-4ba9-b035-78c0d6c10712",
                ExternalPrice = Money.FromDecimal("INR", 599M),
                VariantId = 1,
                Quantity = 1
            };

            var lstLineItems = new List<ILineItemDraft>() { myLineItemDraft };

            var cartDraft = new CartDraft()
            {
                Currency = "INR",
                CustomerId = customer.Id,
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

            // Soma TODO

            // TODO: ADD items to the cart 
            // TODO: ADD discount coupon code to the cart            
            // TODO: RECALCULATE the cart   
            // TODO: ADD default shipping to the cart

            var lineItems = new CartAddLineItemAction()
            {
                Quantity = 1,
                ExternalPrice = myLineItemDraft.ExternalPrice,
                ProductId = "5d1bfcad-bc70-4733-8de0-2f1f22931ee4",
                VariantId = 1,

            };

            var getDiscountCode = await _client.WithApi()
                .WithProjectKey(projectKey)
                .DiscountCodes()
                .WithId("c4fad0d5-6918-4774-bbcc-0a993f39832f")
                .Get()
                .ExecuteAsync();

            var discousntCode = new CartAddDiscountCodeAction
            {

                Code = getDiscountCode.Code

            };

            var recalculate = new CartRecalculateAction()
            {
                UpdateProductData = true,
            };

            var addressDraft = new AddressDraft()
            {

                Country = "DE",
                FirstName = customer?.FirstName,
                LastName = customer?.LastName,

            };

            var shippingAddress = new CartSetShippingAddressAction()
            {
                Address = addressDraft
            };

            var cartUpdate = new CartUpdate()
            {
                Actions = new List<ICartUpdateAction> { lineItems, discousntCode, recalculate, shippingAddress },
                Version = cart?.Version ?? 0
            };

            var updatedCart = await _client.WithApi()
                 .WithProjectKey(projectKey)
                 .Carts()
                 .WithId(cart?.Id)
                 .Post(cartUpdate)
                 .ExecuteAsync();

            // TODO: CREATE a payment 

            var customerResource = new CustomerResourceIdentifier() { Id = customer?.Id };

            var payementDraft = new PaymentDraft()
            {
                AmountPlanned = Money.FromDecimal(updatedCart?.TotalPrice.CurrencyCode, Convert.ToDecimal(updatedCart?.TotalPrice.CentAmount))
            };

            var payment = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Payments()
                .Post(payementDraft)
                .ExecuteAsync();

            Console.WriteLine($"Payment Created with Id: {payment.Id}");

            // TODO: ADD transaction to the payment

            var transactionDraft = new TransactionDraft()
            {
                Timestamp = DateTime.UtcNow,
                Type = ITransactionType.Charge,
                Amount = Money.FromDecimal(updatedCart?.TotalPrice.CurrencyCode, Convert.ToDecimal(updatedCart?.TotalPrice.CentAmount))
            };

            var addTransaction = new PaymentAddTransactionAction()
            {
                Transaction = transactionDraft,
            };

            var updatedPayemnt = new PaymentUpdate()
            {
                Actions = new List<IPaymentUpdateAction> { addTransaction },
                Version = payment?.Version ?? 0,

            };

            var transaction = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Payments()
                .WithId(payment?.Id)
                .Post(updatedPayemnt)
                .ExecuteAsync();


            // TODO: ADD payment to the cart

            var paymentResource = new PaymentResourceIdentifier() { Id = payment?.Id };

            var cartPayment = new CartAddPaymentAction()
            {
                Payment = paymentResource
            };

            var paymentAddedToCart = new CartUpdate()
            {
                Actions = new List<ICartUpdateAction> { cartPayment },
                Version = updatedCart?.Version ?? 0,

            };

            var cartUpdateWithPayment = await _client.WithApi()
                  .WithProjectKey(projectKey)
                  .Carts()
                  .WithId(updatedCart?.Id)
                  .Post(paymentAddedToCart)
                  .ExecuteAsync();

            // TODO: CREATE order

            var cartResource = new CartResourceIdentifier() { Id = updatedCart?.Id };

            var orderDraft = new OrderFromCartDraft()
            {
                Cart = cartResource,
                Version = cartUpdateWithPayment?.Version ?? 0,


            };
            var order = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Orders()
                .Post(orderDraft)
                .ExecuteAsync();

            Console.WriteLine($"Order Created with order number: {order.OrderNumber}");

            // TODO: UPDATE order state to Confirmed

            var changeOrderState = new OrderChangeOrderStateAction()
            {
                OrderState = IOrderState.Confirmed,

            };
            var orderUpdate = new OrderUpdate()
            {
                Actions = new List<IOrderUpdateAction> { changeOrderState },

                Version = order?.Version ?? 0,

            };

            var orderConfirmed = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Orders()
                .WithId(order?.Id)
                .Post(orderUpdate)
                .ExecuteAsync();

            Console.WriteLine($"Order state changed to: {order?.OrderState.Value}");

            // TODO: GET custom workflow state for Order

            var orderPackedState = await _client.WithApi()
             .WithProjectKey(projectKey)
             .States()
             .WithId("3cdbb45d-cad5-4746-8561-bd9cb653a64a")
             .Get()
             .ExecuteAsync();

            var orderShippedState = await _client.WithApi()
              .WithProjectKey(projectKey)
              .States()
              .WithId("14cf5619-0da9-42ec-8f3f-90379bcca007")
              .Get()
              .ExecuteAsync();

            var stateResource2 = new StateReference() { Obj = orderPackedState, Id = orderPackedState.Id };
            var s = new StateResourceIdentifier() { Id = stateResource2.Id };
            var v = new OrderTransitionStateAction()
            {
                State = s,

            };

            // TODO: UPDATE order custom workflow state

            var orderUpdate2 = new OrderUpdate()
            {
                Actions = new List<IOrderUpdateAction> { v },

                Version = orderConfirmed?.Version ?? 0,

            };

            var orderConfirmed2 = await _client.WithApi()
               .WithProjectKey(projectKey)
               .Orders()
               .WithId(orderConfirmed?.Id)
               .Post(orderUpdate2)
               .ExecuteAsync();


            await Task.CompletedTask;
            return $"Order Workflow State changed to: {orderConfirmed2?.State?.Id}";
        }
    }
}
