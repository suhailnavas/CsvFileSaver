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

        //public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        //{
        //    return _baseService.SendAsync<T>(new APIRequest()
        //    {
        //        ApiType = SD.ApiType.POST,
        //        Data = obj,
        //        Url = builderUrl + "api/v1/UsersAuth/login"
        //    });
        //}

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
