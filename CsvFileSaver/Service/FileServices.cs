using CsvFileSaver.Models;
using CsvFileSaver.Service.IService;
using CsvFileSaver.Utility;
using Newtonsoft.Json.Linq;

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

        public async Task<T> GetAllAsync<T>(string token,string userId, string UserRole)
        {
            var ner = new APIRequest()
            {
                ApiType = Constants.ApiType.GET,
                Url = builderUrl + Constants.GetFileRequestEndPoint + string.Format(Constants.GetFileParams, UserRole, userId),
                Token = token
            };

            return await _baseService.SendAsync<T>(ner, true);
        }
        
        public async Task<T> GetRecordsAsync<T>(int fileId, string token)
        {
            var ner = new APIRequest()
            {
                ApiType = Constants.ApiType.GET,
                Url = builderUrl + Constants.GetRecordsRequestEndPoint + string.Format(Constants.GetRecordsParams, fileId),
                Token = token
            };

            return await _baseService.SendAsync<T>(ner, true);
        }

        public Task<T> SedAsync<T>(object obj, string token)
        {
            return _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = Constants.ApiType.POST,
                Data = obj,
                Url = builderUrl + Constants.PostFileEndPoint,
                Token = token
            },true);
        }

        public Task<T> SedRecorsAsync<T>(object obj, string token)
        {
            return _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = Constants.ApiType.POST,
                Data = obj,
                Url = builderUrl + Constants.UploadRecordsEndPoint,
                Token = token
            }, true);
        }
    }
}
