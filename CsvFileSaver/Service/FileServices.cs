using CsvFileSaver.Models;
using CsvFileSaver.Service.IService;
using CsvFileSaver.Utility;

namespace CsvFileSaver.Service
{
    public class FileServices : IFileServices
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

        public async Task<T> GetAllAsync<T>(string token)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = Constants.ApiType.GET,
                Url = builderUrl + Constants.GetFileRequestEndPoint,
                Token = token
            },true);
        }

        public Task<T> SedAsync<T>(object obj)
        {
            return _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = Constants.ApiType.POST,
                Data = obj,
                Url = builderUrl + Constants.PostFileEndPoint
            });
        }
    }
}
