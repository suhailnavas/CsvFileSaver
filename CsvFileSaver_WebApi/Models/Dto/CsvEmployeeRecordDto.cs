namespace CsvFileSaver_WebApi.Models.Dto
{
    public class CsvEmployeeRecordDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public double Salary { get; set; }
    }
}
