namespace Noodle_Assignment.Interface
{
    public interface IProductSelectionsService
    {
        Task<string> ExecuteAsync(ProductSelectionModel productSelectionModel);
    }
}
