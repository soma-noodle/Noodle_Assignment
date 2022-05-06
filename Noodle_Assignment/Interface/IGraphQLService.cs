namespace Noodle_Assignment.Interface
{
    public interface IGraphQLService
    {
        Task<List<string>> ExecuteAsync();
    }
}
