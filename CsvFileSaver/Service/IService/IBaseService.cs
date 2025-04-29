using CsvFileSaver.Models;

namespace CsvFileSaver.Service.IService
{
    public interface IBaseService
    {
        public APIResponse responseModel { get; set; }
        public Task<T> SendAsync<T>(APIRequest apiRequest,bool isTokenRequired = false);
    }
}
