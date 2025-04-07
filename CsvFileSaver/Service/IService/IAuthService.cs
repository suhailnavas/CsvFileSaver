using CsvFileSaver.Models.Dto;

namespace CsvFileSaver.Service.IService
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDto objTologin);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
    }
}
