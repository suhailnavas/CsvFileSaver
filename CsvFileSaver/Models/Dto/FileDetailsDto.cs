using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CsvFileSaver.Models.Dto
{
    public class FileDetailsDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string status { get; set; }
        public bool IsUpdated { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
