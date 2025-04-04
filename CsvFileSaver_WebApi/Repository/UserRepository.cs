using AutoMapper;
using CsvFileSaver_WebApi.Data;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;

namespace CsvFileSaver_WebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public bool IsUniqueUser(string email)
        {
            var user = _db.UserDetails.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<RegisterationRequest> Register(RegisterationRequestDTO registerationRequestDTO)
        {

            RegisterationRequest user = new()
            {
                Email = registerationRequestDTO.Email,
                Password = registerationRequestDTO.Password,
            };
            _db.UserDetails.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
    
}
