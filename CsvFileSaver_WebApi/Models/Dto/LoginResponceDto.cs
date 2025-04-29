namespace CsvFileSaver_WebApi.Models.Dto
{
    public class LoginResponceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }

    }
}
