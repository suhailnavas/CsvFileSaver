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

        public enum Status
        {
            Not_Updated,
            Processing,
            Completed,
        }

        public static string SessionToken = "JWTToken";
        public static string AccessToken = "JWTToken";
        public static string RefreshToken = "RefreshToken";

        //Base Url connection String
        public static string CsvFileSaverServiceUrl = "ServiceUrls:CsvFileSaverAPI";

        //Url EndPoints
        public static string LoginRegisterEndPoint = "api/v1/Login/register";
        public static string LoginRequestEndPoint = "api/v1/Login/Login";
        public static string GetFileRequestEndPoint = "api/v1/FileUpload/GetFiles";
        public static string PostFileEndPoint = "api/v1/FileUpload/FileUploade";
        public static string UploadRecordsEndPoint = "api/v1/FileUpload/UploadRecords";

        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}
