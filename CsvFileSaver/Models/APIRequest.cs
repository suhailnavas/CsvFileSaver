using System.Net.Mime;
using System.Security.AccessControl;
using static CsvFileSaver.Utility.Constants;
using  CsvFileSaver.Utility;
using ContentType = CsvFileSaver.Utility.Constants.ContentType;

namespace CsvFileSaver.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
        public ContentType contentType { get; set; } = ContentType.Json;
    }
}
