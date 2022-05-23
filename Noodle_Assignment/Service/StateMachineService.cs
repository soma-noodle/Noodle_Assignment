namespace Noodle_Assignment.Service
{
    public class StateMachineService : IStateMachineService
    {
        private readonly IClient _client;
        private readonly string projectKey;

        public StateMachineService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task<string> ExecuteAsync(StateMachineModel model)
        {
            // TODO: CREATE OrderPacked stateDraft, state
            var stateOrde1Draft = new StateDraft
            {
                Key = model.InitialOrderStateKey,
                Initial = true,
                Name = new LocalizedString { { "en", model.InitialOrderStateName } },
                Type = IStateTypeEnum.OrderState
            };

            var order1Response = await _client.WithApi().WithProjectKey(projectKey)
                .States()
                .Post(stateOrde1Draft)
                .ExecuteAsync();

            // TODO: CREATE OrderShipped stateDraft, state
            var stateOrder2Draft = new StateDraft
            {
                Key = model.TransitionedOrderStateKey,
                Initial = false,
                Name = new LocalizedString { { "en", model.TrasitionedOrderStateName } },
                Type = IStateTypeEnum.OrderState
            };

            var order2Response = await _client.WithApi().WithProjectKey(projectKey)
                .States()
                .Post(stateOrder2Draft)
                .ExecuteAsync();

            // TODO: UPDATE packedState to transit to stateShipped

            var stateResource = new StateResourceIdentifier() { Id = order2Response?.Id };

            var action = new StateSetTransitionsAction()
            {
                Transitions = new List<IStateResourceIdentifier>() { stateResource }
            };

            var stateUpdate = new StateUpdate()
            {
                Actions = new List<IStateUpdateAction>() { action },
                Version = order1Response?.Version ?? 0
            };

            var updatedOrder1response = await _client.WithApi().WithProjectKey(projectKey)
                .States()
                .WithId(order1Response?.Id)
                .Post(stateUpdate)
                .ExecuteAsync();

            return "stateOrder2 Id : " + order2Response?.Id + ", stateOrder1 transition to:  " + updatedOrder1response?.Transitions[0].Id;
        }
    }
}
