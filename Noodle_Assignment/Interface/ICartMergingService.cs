namespace Noodle_Assignment.Interface
{
    public interface ICartMergingService
    {
        Task<string> ExecuteAsync(CartMergeModel cartMergeModel);
    }
}
