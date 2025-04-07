using CsvFileSaver.Models;
using CsvFileSaver.Models.Dto;
using CsvFileSaver.Service.IService;
using CsvFileSaver.Utility;

namespace CsvFileSaver.Service
{
    public class AuthService:IAuthService
    {
        private readonly IBaseService _baseService;
        private string builderUrl;
        public AuthService(IConfiguration configuration, IBaseService baseService)
        {
            _baseService = baseService;
            builderUrl = configuration.GetValue<string>(Constants.CsvFileSaverServiceUrl);

        }

        public Task<T> LoginAsync<T>(LoginRequestDto obj)
        {
            return _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = Constants.ApiType.POST,
                Data = obj,
                Url = builderUrl + Constants.LoginRequestEndPoint
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
        {
            return _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = Constants.ApiType.POST,
                Data = obj,
                Url = builderUrl + Constants.LoginRegisterEndPoint
            });
        }
    }
}
