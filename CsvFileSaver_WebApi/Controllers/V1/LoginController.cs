using AutoMapper;
using CsvFileSaver_WebApi.Model;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using CsvFileSaver_WebApi.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CsvFileSaver_WebApi.Controllers.V1
{
    [Route(Constants.LoginControllerRute)]
    [ApiController]
    [ApiVersion(Constants.ApiVersionOnePointZero)]
    public class LoginController : ControllerBase
    {
        private readonly APIResponse _response;
        private readonly ILoginRepository _dbLogin;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        public LoginController(ILoginRepository dbLogin, IMapper mapper, IUserRepository userRepo)
        {
            _response = new APIResponse();
            _dbLogin = dbLogin;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Login")]
        public async Task<ActionResult<APIResponse>> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                if (string.IsNullOrEmpty(loginRequestDto.Email))
                {
                    return BadRequest();
                }
                else
                {
                    var loginResponce = await _userRepo.Login(loginRequestDto);
                    if (loginResponce == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages.Add("Invalid User Name Or Password");
                        return NotFound();
                    }
                    else
                    {
                        _response.Result = loginResponce;
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        return Ok(_response);
                    }                   
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Email);
            if (!ifUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            var user = await _userRepo.Register(model);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
