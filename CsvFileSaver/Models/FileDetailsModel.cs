namespace CsvFileSaver.Models
{
    public class FileDetailsModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string status { get; set; }
        public bool IsUpdated { get; set; }
        public byte[] Content { get; set; }   
        public string ContentType { get; set; }  
       
    }
}
