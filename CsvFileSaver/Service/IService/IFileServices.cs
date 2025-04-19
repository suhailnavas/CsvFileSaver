namespace CsvFileSaver.Service.IService
{
    public interface IFileServices
    {
        Task<T> SedAsync<T>(object obj, string token);
        Task<T> SedRecorsAsync<T>(object obj, string token);
        Task<T> GetAllAsync<T>(string token,string userId ,string userRole);
    }
}
