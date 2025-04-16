namespace CsvFileSaver.Service.IService
{
    public interface IFileServices
    {
        Task<T> SedAsync<T>(object obj);
        Task<T> GetAllAsync<T>(string token);
    }
}
