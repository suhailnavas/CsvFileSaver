using CsvFileSaver.Models;
using CsvFileSaver.Service.IService;
using CsvFileSaver.Utility;
using Newtonsoft.Json;
using System.Text;

namespace CsvFileSaver.Service
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory HttpClient;

        public BaseService(IHttpClientFactory httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = HttpClient.CreateClient("BustanAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }

                switch (apiRequest.ApiType)
                {
                    case Constants.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Constants.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Constants.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponce = null;
                apiResponce = await client.SendAsync(message);
                var apiContent = await apiResponce.Content.ReadAsStringAsync();
                var APIResponce = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponce;
            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }  
}
