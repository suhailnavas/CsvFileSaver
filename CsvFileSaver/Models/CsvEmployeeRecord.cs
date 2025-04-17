using System.Text.Json.Serialization;

namespace CsvFileSaver.Models
{
    public class CsvEmployeeRecord
    {               
        public string FirstName { get; set; }      
        public string LastName { get; set; }      
        public string Email { get; set; }         
        public string Department { get; set; }     
        public double Salary { get; set; }       
    }
}
