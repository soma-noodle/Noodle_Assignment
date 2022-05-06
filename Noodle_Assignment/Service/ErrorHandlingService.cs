namespace Noodle_Assignment.Service
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public ErrorHandlingService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            var customerKeyMayOrMayNotExist = "customer-michele-WRONG-KEY";
            try
            {
                //get non existing customer by key
                var customer = await _client.WithApi().WithProjectKey(projectKey)
               .Customers()
               .WithKey(customerKeyMayOrMayNotExist)
               .Get()
               .ExecuteAsync();

                return "Customer Name : " + customer?.FirstName + " " + customer?.LastName;
            }
            catch (NotFoundException e)
            {
                return e.Body;
                //throw;
            }
        }
    }
}
