using CsvFileSaver.Models.Dto;

namespace CsvFileSaver.Service.IService
{
    public interface IAuthService
    {
       // Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
    }
}
