namespace Noodle_Assignment.Service
{
    public class CustomTypesService : ICustomTypesService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public CustomTypesService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync(CustomTypeModel customTypeModel)
        {
            var customField = new FieldDefinition()
            {
                Name = customTypeModel.CustomField.CustomFieldName,
                Required = customTypeModel.CustomField.Required,
                Label = new LocalizedString { { "en", customTypeModel.CustomField.Label } },
                Type = new CustomFieldStringType() { Name = "String" }
            };

            var customFieldDefination = new List<IFieldDefinition>() { customField };

            var typeDraft = new TypeDraft()
            {


                Name = new LocalizedString { { "en", customTypeModel.TypeName } },
                Key = customTypeModel.Key,
                ResourceTypeIds = new List<IResourceTypeId> { IResourceTypeId.Customer },
                Description = new LocalizedString { { "desc", customTypeModel.Description } },
                FieldDefinitions = customFieldDefination,


            };

            var customtype = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Types()
                .Post(typeDraft)
                .ExecuteAsync();

            Console.WriteLine($"New custom type has been created with Id: {customtype.Id}");
            var customers = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Customers()
                .Get()
                .ExecuteAsync();

            foreach (var customer in customers.Results)
            {
                var customeField = new CustomerSetCustomTypeAction()
                {
                    Type = new TypeResourceIdentifier() { Id = customtype.Id },

                };
                var customerUpdateAction = new CustomerUpdate()
                {
                    Actions = new List<ICustomerUpdateAction>() { customeField },
                    Version = customer.Version,

                };

                var updatedCustomer = await _client.WithApi()
                    .WithProjectKey(projectKey)
                    .Customers()
                    .WithId(customer.Id)
                    .Post(customerUpdateAction)
                    .ExecuteAsync();
            }

            return "Custom Types exercise completed successfully";
        }
    }
}
