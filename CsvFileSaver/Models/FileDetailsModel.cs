namespace CsvFileSaver.Models
{
    public class FileDetailsModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string status { get; set; }
        public string Action { get; set; }

        public byte[] Content { get; set; }   // The actual file content
        public string ContentType { get; set; }  // e.g., text/csv
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
