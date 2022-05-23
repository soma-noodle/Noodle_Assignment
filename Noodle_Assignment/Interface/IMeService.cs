namespace Noodle_Assignment.Interface
{
    public interface IMeService
    {
        Task<string> ExecuteAsync(MeClientModel meClient);
    }
}
