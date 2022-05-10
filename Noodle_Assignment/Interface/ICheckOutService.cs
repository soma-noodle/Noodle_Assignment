namespace Noodle_Assignment.Interface
{
    public interface ICheckOutService
    {
        Task<string> ExecuteAsync(CheckoutModel model);
    }
}
