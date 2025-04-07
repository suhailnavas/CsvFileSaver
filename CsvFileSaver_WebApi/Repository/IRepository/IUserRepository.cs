using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;

namespace CsvFileSaver_WebApi.Repository.IRepository
{
    public interface IUserRepository
    {
            bool IsUniqueUser(string username);
            Task<LoginResponceDto> Login(LoginRequestDto loginRequestDTO);
            Task<RegisterationRequest> Register(RegisterationRequestDTO registerationRequestDTO);
            //Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);        
    }
}
