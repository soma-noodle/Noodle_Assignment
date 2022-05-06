namespace Noodle_Assignment.Service
{
    public class UpdateGroupService : IUpdateGroupService
    {
        private readonly IClient _client;
        private readonly string projectKey;
        //private const string _customerId = "9215ed81-83a8-4741-b8cd-2a8f51dbce1a"; // fistname: SSS7 lastname: PPP7 email: soma7@example.com 
        //private const string _customerGroupId = "84dea057-6906-42a7-8c37-a72ead3c4b86"; // diamond group

        public UpdateGroupService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync(UpdateServiceModel model)
        {
            // Read customer using Id 
            // ex: 9215ed81-83a8-4741-b8cd-2a8f51dbce1a fistname: SSS7 lastname: PPP7 email: soma7@example.com

            var customer = await _client.WithApi().WithProjectKey(projectKey)
              .Customers()
              .WithId(model.CustomerId)
              .Get()
              .ExecuteAsync();

            //var customerGroup = await _client.WithApi().WithProjectKey(projectKey)
            //  .CustomerGroups()
            //  .WithId(_customerGroupId)
            //  .Get()
            //  .ExecuteAsync();

            // TODO: SET customerGroup for the customer

            var action = new CustomerSetCustomerGroupAction()
            {
                CustomerGroup = new CustomerGroupResourceIdentifier() { Id = model.CustomerGroupId }
            };

            var customerUpdate = new CustomerUpdate()
            {
                Actions = new List<ICustomerUpdateAction>() { action },
                Version = customer?.Version ?? 0,
            };

            var updatedCustomer = await _client.WithApi().WithProjectKey(projectKey)
              .Customers()
              .WithId(model.CustomerId)
              .Post(customerUpdate)
              .ExecuteAsync();

            var updatedCustomerId = updatedCustomer?.Id;
            var updatedCustomerGroupId = updatedCustomer?.CustomerGroup?.Id;

            //Console.WriteLine($"customer {updatedCustomer.Id} in customer group {updatedCustomer.CustomerGroup.Id}");

            await Task.CompletedTask;
            return "customer " + updatedCustomerId + " in customer group " + updatedCustomerGroupId;
        }
    }
}
