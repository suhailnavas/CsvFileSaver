using CsvFileSaver.Models;
using CsvFileSaver.Service.IService;
using CsvFileSaver.Utility;

namespace CsvFileSaver.Service
{
    public class FileServices
    {

        private readonly IBaseService _baseService;
        private string builderUrl;
        private IHttpContextAccessor _contextAccessor;
        public FileServices(IConfiguration configuration, IBaseService baseService, IHttpContextAccessor contextAccessor)
        {
            _baseService = baseService;
            builderUrl = configuration.GetValue<string>(Constants.CsvFileSaverServiceUrl);
            _contextAccessor = contextAccessor;
        }

        public Task<T> LoginAsync<T>(object obj)
        {
            return _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = Constants.ApiType.POST,
                Data = obj,
                Url = builderUrl + Constants.LoginRequestEndPoint
            });
        }
    }
}
