using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CsvFileSaver_WebApi.Models
{
    public class FileDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public bool IsUpdated { get; set; }
    }

}
