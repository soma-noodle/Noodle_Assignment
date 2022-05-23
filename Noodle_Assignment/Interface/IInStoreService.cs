namespace Noodle_Assignment.Interface
{
    public interface IInStoreService
    {
        Task<string> ExecuteAsync(InStoreModel inStoreModel);
    }
}
