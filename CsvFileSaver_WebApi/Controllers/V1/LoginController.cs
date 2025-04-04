using AutoMapper;
using Azure;
using CsvFileSaver_WebApi.Model;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using CsvFileSaver_WebApi.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        [HttpGet(Constants.id, Name = Constants.GetOnedata)]
        public async Task<ActionResult<APIResponse>> GetOnedata(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest();
                }
                else
                {
                    var builder = await _dbLogin.GetAsync(u => u.UserId == id);
                    if (builder == null)
                    {
                        return NotFound();
                    }
                    _response.Result = await _dbLogin.GetAsync(u => u.UserId == id);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Login([FromBody]LoginDto userLogin)
        {
            try
            {
                if (userLogin == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (userLogin.UserId > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                //createbuilderDTO.Id = _db.builder.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
                Login usertdata = _mapper.Map<Login>(userLogin);
                await _dbLogin.CreateAsync(usertdata);
                _response.Result = userLogin;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute(Constants.GetOnedata, userLogin.UserId, userLogin);
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
