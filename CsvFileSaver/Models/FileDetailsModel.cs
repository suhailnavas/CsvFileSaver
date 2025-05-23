﻿using System.Text.Json.Serialization;

namespace CsvFileSaver.Models
{
    public class FileDetailsModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string status { get; set; }
        public bool IsUpdated { get; set; }
        public byte[] Content { get; set; }   
        public string ContentType { get; set; }  
        public string Base64Content { get; set; }        
    }
}
