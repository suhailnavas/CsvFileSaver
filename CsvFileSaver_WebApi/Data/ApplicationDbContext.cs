using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CsvFileSaver_WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }     
        public DbSet<RegisterationRequest> UserDetails { get; set; }       
        public DbSet<FileDetails> UploadFileDetails { get; set; }       
        public DbSet<CsvEmployeeRecord> CsvEmployeeRecords { get; set; }       
    }
}
