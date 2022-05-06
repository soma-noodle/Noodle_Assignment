namespace Noodle_Assignment.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public SubscriptionService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            //create destination
            var destination = new GoogleCloudPubSubDestination()
            {
                Type = "GoogleCloudPubSub",
                ProjectId = "ct-support",
                Topic = "training-subscription-sample"
            };

            var subscriptionDraft = new SubscriptionDraft() { Destination = destination };

            // TODO: CREATE the subscription
            var response = await _client.WithApi().WithProjectKey(projectKey)
               .Subscriptions()
               .Post(subscriptionDraft)
               .ExecuteAsync();

            //Console.WriteLine($"a new subscription created with Id {subscription.Id}");

            return "a new subscription created with Id " + response?.Id;
        }
    }
}
