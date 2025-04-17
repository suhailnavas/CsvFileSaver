using CsvFileSaver.Models.Dto;

namespace CsvFileSaver.Models
{
    public class FilesAndRecordsDto
    {
        public FileDetailsDto FileDetails { get; set; }
        public List<CsvEmployeeRecordDto> RecordsDetails { get; set; }
    }
}
