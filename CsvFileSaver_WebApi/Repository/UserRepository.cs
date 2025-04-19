using AutoMapper;
using CsvFileSaver_WebApi.Data;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CsvFileSaver_WebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
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

        public async Task<LoginResponceDto?> Login(LoginRequestDto loginRequestDTO)
        {
            if (string.IsNullOrEmpty(loginRequestDTO.Email) || string.IsNullOrEmpty(loginRequestDTO.Password))
                return null ;

            var user = _db.UserDetails.FirstOrDefault(x => x.Email == loginRequestDTO.Email);
   
            if (user == null ||!loginRequestDTO.Password.Equals(user.Password))
            {
                return null;
            }
           
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityInMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTime = DateTime.Now.AddMinutes(tokenValidityInMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name,user.Name)
                }),
                Expires = tokenExpiryTime,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accesToken = tokenHandler.WriteToken(securityToken);

            LoginResponceDto response = new LoginResponceDto
            {
                Id = user.UserId,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                AccessToken = accesToken
            };

            return await Task.FromResult(response);
        }

        public async Task<RegisterationRequest> Register(RegisterationRequestDTO registerationRequestDTO)
        {

            RegisterationRequest user = new()
            {
                Name = registerationRequestDTO.Name,
                Email = registerationRequestDTO.Email,
                Password = registerationRequestDTO.Password,
                Role = registerationRequestDTO.Role,
            };
            _db.UserDetails.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
    
}
