using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsvFileSaver_WebApi.Models.Dto
{
    public class LoginDto
    {
       //public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
