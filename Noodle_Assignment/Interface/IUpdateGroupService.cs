namespace Noodle_Assignment.Interface
{
    public interface IUpdateGroupService
    {
        Task<string> ExecuteAsync(UpdateServiceModel model);
    }
}
