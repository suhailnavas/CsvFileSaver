namespace CsvFileSaver.Service.IService
{
    public interface IFileServices
    {
        Task<T> SedAsync<T>(object objTologin);
    }
}
