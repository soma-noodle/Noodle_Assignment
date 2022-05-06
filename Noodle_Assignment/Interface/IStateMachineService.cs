namespace Noodle_Assignment.Interface
{
    public interface IStateMachineService
    {
        Task<string> ExecuteAsync(StateMachineModel model);
    }
}
