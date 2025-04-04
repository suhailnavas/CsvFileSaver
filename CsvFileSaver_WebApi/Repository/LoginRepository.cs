using CsvFileSaver_WebApi.Data;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;

namespace CsvFileSaver_WebApi.Repository
{
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        private readonly ApplicationDbContext _db;

        public LoginRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
