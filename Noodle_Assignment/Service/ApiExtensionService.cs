﻿namespace Noodle_Assignment.Service
{
    public class ApiExtensionService : IApiExtensionService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public ApiExtensionService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            await Task.CompletedTask;
            return "";
        }
    }
}
