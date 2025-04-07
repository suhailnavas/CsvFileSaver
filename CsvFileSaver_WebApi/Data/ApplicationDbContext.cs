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
        //public DbSet<Login> LoginUserDetails { get; set; }       
        public DbSet<RegisterationRequest> UserDetails { get; set; }       
    }
}
