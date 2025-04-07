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

        //Base Url connection String
        public static string CsvFileSaverServiceUrl = "ServiceUrls:CsvFileSaverAPI";

        //Url EndPoints
        public static string LoginRegisterEndPoint = "api/v1/Login/register";
        public static string LoginRequestEndPoint = "api/v1/Login/Login";

        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}
