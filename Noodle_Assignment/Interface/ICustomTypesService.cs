namespace Noodle_Assignment.Interface
{
    public interface ICustomTypesService
    {
        Task<string> ExecuteAsync(CustomTypeModel customTypeModel);
    }
}
