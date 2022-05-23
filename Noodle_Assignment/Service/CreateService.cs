namespace Noodle_Assignment.Service
{
    public class CreateService : ICreateService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public CreateService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync(CustomerModel customerModel)
        {

            // CREATE customer draft
            // we are reading customer draft from request body

            // Following is the example structure for customer draft object 

            //var customerDraft = new CustomerDraft
            //{
            //    CustomerNumber = "6",
            //    Email = "soma6test@example.com",
            //    Password = "password",
            //    Key = Guid.NewGuid().ToString("n").Substring(0, 8),
            //    FirstName = "SSS",
            //    LastName = "PPP",               
            //    DefaultShippingAddress = 0,
            //    DefaultBillingAddress = 0
            //};

            var customerDraft = new CustomerDraft
            {
                Email = customerModel.Email,
                Password = customerModel.Password,
                Key = Guid.NewGuid().ToString("n").Substring(0, 8),
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                Addresses = new List<IBaseAddress>{
                        new AddressDraft {
                            Country = customerModel.Country,
                    }
                },
                DefaultShippingAddress = 0,
                DefaultBillingAddress = 0
            };

            // TODO: SIGNUP a customer

            var response = await _client.WithApi().WithProjectKey(projectKey)
               .Customers()
               .Post(customerDraft)
               .ExecuteAsync();

            var customer = response?.Customer;
            //Console.WriteLine($"Customer Created with Id : {customer?.Id} and Key : {customer?.Key} and Email Verified: {customer?.IsEmailVerified}");

            // TODO: CREATE a email verfification token

            var response1 = await _client.WithApi().WithProjectKey(projectKey)
               .Customers().EmailToken()
               .Post(new CustomerCreateEmailToken() { Id = customer?.Id, TtlMinutes = 5 })
               .ExecuteAsync();

            // TODO: CONFIRM CustomerEmail

            var response2 = await _client.WithApi().WithProjectKey(projectKey)
               .Customers()
               .EmailConfirm()
               .Post(new CustomerEmailVerify() { TokenValue = response1?.Value })
               .ExecuteAsync();

            var isEmailVerified = response2?.IsEmailVerified;

            //Console.WriteLine($"Is Email Verified:{retrievedCustomer.IsEmailVerified}");

            await Task.CompletedTask;
            return "Email Verified ==> " + isEmailVerified;
        }

        public string GenerateRandomString()
        {
            int length =7;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }
    }
}
