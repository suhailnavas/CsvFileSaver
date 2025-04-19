using System.Data;

namespace CsvFileSaver.Utility
{
    public static class Constants
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }

        public static string SessionToken = "JWTToken";
        public static string AccessToken = "JWTToken";
        public static string RefreshToken = "RefreshToken";
        public static string FileUploaded = "File uploaded";
        public static string UserRole = "UserRole";
        public static string UserName = "UserName";
        public static string UserId = "UserId";
        public static string Admin = "Admin";

        //Base Url connection String
        public static string CsvFileSaverServiceUrl = "ServiceUrls:CsvFileSaverAPI";

        //Url EndPoints
        public static string LoginRegisterEndPoint = "api/v1/Login/register";
        public static string LoginRequestEndPoint = "api/v1/Login/Login";
        public static string GetFileRequestEndPoint = "api/v1/FileUpload/GetFiles";
        public static string PostFileEndPoint = "api/v1/FileUpload/FileUploade";
        public static string UploadRecordsEndPoint = "api/v1/FileUpload/UploadRecords";
        public static string GetFileParams = "?role={0}&userId={1}";

        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}
